using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Warp.NET.SourceGen.Extensions;
using Warp.NET.SourceGen.Utils;

namespace Warp.NET.SourceGen.Generators.Game;

[Generator]
public class GameGenerator : IIncrementalGenerator
{
	private const string _namespace = $"%{nameof(_namespace)}%";
	private const string _gameObjectListFields = $"%{nameof(_gameObjectListFields)}%";
	private const string _gameObjectListProperties = $"%{nameof(_gameObjectListProperties)}%";
	private const string _gameObjectListAdds = $"%{nameof(_gameObjectListAdds)}%";
	private const string _gameObjectListRemoves = $"%{nameof(_gameObjectListRemoves)}%";

	private const string _gameTemplate = $$"""
		using System;
		using System.Collections.Generic;
		using System.Text;
		using Warp.NET;

		namespace {{_namespace}};

		public sealed partial class Game : GameBase, IGameBase<Game>
		{
			private static Game? _self;

			{{_gameObjectListFields}}

			public static Game Self
			{
				get => _self ?? throw new InvalidOperationException("Game is not initialized.");
				set
				{
					if (_self != null)
						throw new InvalidOperationException("Game is already initialized.");

					_self = value;
				}
			}

			{{_gameObjectListProperties}}

			public static Game Construct()
			{
				return new();
			}

			protected override void HandleAdds({{Constants.RootNamespace}}.GameObjects.IGameObject gameObject)
			{
				base.HandleAdds(gameObject);

				{{_gameObjectListAdds}}
			}

			protected override void HandleRemoves({{Constants.RootNamespace}}.GameObjects.IGameObject gameObject)
			{
				base.HandleRemoves(gameObject);

				{{_gameObjectListRemoves}}
			}
		}
		""";

	private static readonly TypeName _generateGameObjectListAttributeTypeName = new("GenerateGameObjectListAttribute");
	private static readonly TypeName _generateGameAttributeTypeName = new("GenerateGameAttribute");

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// TODO: Fix generating duplicate attributes. https://andrewlock.net/creating-a-source-generator-part-8-solving-the-source-generator-marker-attribute-problem-part2/
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(_generateGameObjectListAttributeTypeName.Type, SourceBuilderUtils.GenerateAttribute(AttributeTargets.Class | AttributeTargets.Interface, _generateGameObjectListAttributeTypeName.Type)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(_generateGameAttributeTypeName.Type, SourceBuilderUtils.GenerateAttribute(AttributeTargets.Class, _generateGameAttributeTypeName.Type)));

		// ! LINQ query filters out null values.
		IncrementalValueProvider<ImmutableArray<ClassDeclarationSyntax>> gameDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (sn, _) => sn is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
				transform: static (ctx, _) => ctx.GetTypeWithAttribute<ClassDeclarationSyntax>(_generateGameAttributeTypeName.FullName))
			.Where(static m => m is not null)
			.Collect()!;

		// ! LINQ query filters out null values.
		IncrementalValueProvider<ImmutableArray<TypeDeclarationSyntax>> gameObjectListDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (sn, _) => sn is ClassDeclarationSyntax { AttributeLists.Count: > 0 } or InterfaceDeclarationSyntax { AttributeLists.Count: > 0 },
				transform: static (ctx, _) => ctx.GetTypeWithAttribute<TypeDeclarationSyntax>(_generateGameObjectListAttributeTypeName.FullName))
			.Where(static m => m is not null)
			.Collect()!;

		IncrementalValueProvider<(ImmutableArray<ClassDeclarationSyntax> Games, ImmutableArray<TypeDeclarationSyntax> GameObjectLists)> data = gameDeclarations.Combine(gameObjectListDeclarations);
		IncrementalValueProvider<(Compilation Compilation, (ImmutableArray<ClassDeclarationSyntax> Games, ImmutableArray<TypeDeclarationSyntax> GameObjectLists) Data)> compilation = context.CompilationProvider.Combine(data);

		context.RegisterSourceOutput(
			compilation,
			static (spc, source) => Execute(source.Compilation, source.Data, spc));

		static void Execute(
			Compilation compilation,
			(ImmutableArray<ClassDeclarationSyntax> Games, ImmutableArray<TypeDeclarationSyntax> GameObjectLists) data,
			SourceProductionContext context)
		{
			if (data.Games.Length != 1)
				return;

			string? gameNamespace = compilation.AssemblyName;
			if (gameNamespace == null)
				return;

			List<GameObjectList> gameObjectLists = new();
			if (!data.GameObjectLists.IsDefaultOrEmpty)
				gameObjectLists = compilation.GetTypeDataFromName(data.GameObjectLists.Distinct(), s => new GameObjectList(s), context.CancellationToken);

			string sourceBuilder = _gameTemplate
				.Replace(_namespace, gameNamespace)
				.Replace(_gameObjectListFields, string.Join(Constants.NewLine, gameObjectLists.ConvertAll(s => $"private readonly List<{s.FullTypeName}> {s.FieldName} = new();")).IndentCode(1))
				.Replace(_gameObjectListProperties, string.Join(Constants.NewLine, gameObjectLists.ConvertAll(s => $"public IReadOnlyList<{s.FullTypeName}> {s.PropertyName} => {s.FieldName};")).IndentCode(1))
				.Replace(_gameObjectListAdds, string.Join(Constants.NewLine, gameObjectLists.ConvertAll(s => GenerateHandles(s, "Add"))).IndentCode(2))
				.Replace(_gameObjectListRemoves, string.Join(Constants.NewLine, gameObjectLists.ConvertAll(s => GenerateHandles(s, "Remove"))).IndentCode(2));

			context.AddSource("Game", SourceBuilderUtils.Build(sourceBuilder));
		}

		static string GenerateHandles(GameObjectList gameObjectList, string handleMethodName)
		{
			return $$"""
				if (gameObject is {{gameObjectList.FullTypeName}} {{gameObjectList.VariableName}})
					{{gameObjectList.FieldName}}.{{handleMethodName}}({{gameObjectList.VariableName}});
				""";
		}
	}
}
