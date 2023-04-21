using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Warp.NET.SourceGen.Extensions;
using Warp.NET.SourceGen.Utils;

namespace Warp.NET.SourceGen.Generators.GameObject;

[Generator]
public class GameObjectGenerator : IIncrementalGenerator
{
	private const string _namespace = $"%{nameof(_namespace)}%";
	private const string _className = $"%{nameof(_className)}%";
	private const string _statePrepareUpdates = $"%{nameof(_statePrepareUpdates)}%";
	private const string _statePrepareRenders = $"%{nameof(_statePrepareRenders)}%";
	private const string _childAdds = $"%{nameof(_childAdds)}%";
	private const string _childRemoves = $"%{nameof(_childRemoves)}%";

	private const string _gameObjectTemplate = $$"""
		namespace {{_namespace}};

		public partial class {{_className}}
		{
			public override void PrepareUpdateInterpolation()
			{
				base.PrepareUpdateInterpolation();

				{{_statePrepareUpdates}}
			}

			public override void PrepareRenderInterpolation()
			{
				base.PrepareRenderInterpolation();

				{{_statePrepareRenders}}
			}

			public override void AddChildren()
			{
				base.AddChildren();

				{{_childAdds}}
			}

			public override void RemoveChildren()
			{
				base.RemoveChildren();

				{{_childRemoves}}
			}
		}
		""";

	private static readonly TypeName _gameObjectAttributeTypeName = new("GenerateGameObjectAttribute");
	private static readonly TypeName _childAttributeTypeName = new("GenerateChildAttribute");
	private static readonly TypeName _interpolationAttributeTypeName = new("GenerateInterpolationAttribute");

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(_gameObjectAttributeTypeName.Type, SourceBuilderUtils.GenerateAttribute(AttributeTargets.Class, _gameObjectAttributeTypeName.Type)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(_childAttributeTypeName.Type, SourceBuilderUtils.GenerateAttribute(AttributeTargets.Property | AttributeTargets.Field, _childAttributeTypeName.Type)));
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(_interpolationAttributeTypeName.Type, SourceBuilderUtils.GenerateAttribute(AttributeTargets.Property | AttributeTargets.Field, _interpolationAttributeTypeName.Type)));

		// ! LINQ query filters out null values.
		IncrementalValuesProvider<ClassDeclarationSyntax> gameObjectTypeDeclarations = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (sn, _) => sn is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
				transform: static (ctx, _) => ctx.GetTypeWithAttribute<ClassDeclarationSyntax>(_gameObjectAttributeTypeName.FullName))
			.Where(static m => m is not null)!;

		IncrementalValueProvider<(Compilation Compilation, ImmutableArray<ClassDeclarationSyntax> GameObjects)> compilation = context.CompilationProvider.Combine(gameObjectTypeDeclarations.Collect());

		context.RegisterSourceOutput(compilation, static (spc, source) => Execute(source.Compilation, source.GameObjects, spc));

		static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> gameObjectDeclarations, SourceProductionContext context)
		{
			List<GameObject> gameObjects = new();
			if (!gameObjectDeclarations.IsDefaultOrEmpty)
				gameObjects = GetGameObjectData(compilation, gameObjectDeclarations.Distinct(), context.CancellationToken);

			foreach (GameObject gameObject in gameObjects)
			{
				string sourceBuilder = _gameObjectTemplate
					.Replace(_namespace, gameObject.Namespace)
					.Replace(_className, gameObject.TypeName)
					.Replace(_statePrepareUpdates, string.Join(Constants.NewLine, gameObject.States.ConvertAll(s => $"{s.StateName}.PrepareUpdate();")).IndentCode(2))
					.Replace(_statePrepareRenders, string.Join(Constants.NewLine, gameObject.States.ConvertAll(s => $"{s.StateName}.PrepareRender();")).IndentCode(2))
					.Replace(_childAdds, string.Join(Constants.NewLine, gameObject.Children.ConvertAll(c => $"{c.MemberName}?.Add();")).IndentCode(2))
					.Replace(_childRemoves, string.Join(Constants.NewLine, gameObject.Children.ConvertAll(c => $"{c.MemberName}?.Remove();")).IndentCode(2));

				context.AddSource(gameObject.TypeName, SourceBuilderUtils.Build(sourceBuilder));
			}
		}
	}

	private static List<GameObject> GetGameObjectData(Compilation compilation, IEnumerable<TypeDeclarationSyntax> types, CancellationToken cancellationToken)
	{
		List<GameObject> gameObjects = new();
		foreach (TypeDeclarationSyntax tds in types)
		{
			cancellationToken.ThrowIfCancellationRequested();

			string? gameObjectTypeName = tds.GetFullTypeName(compilation);
			if (gameObjectTypeName == null)
				continue;

			List<GameObjectInterpolationState> states = new();
			List<GameObjectChild> children = new();
			foreach (PropertyDeclarationSyntax property in tds.ChildNodes().OfType<PropertyDeclarationSyntax>())
			{
				if (property.GetAttributeFromMember(_childAttributeTypeName.Type) != null)
					children.Add(new(property.Identifier.ValueText));
				else if (property.GetAttributeFromMember(_interpolationAttributeTypeName.Type) != null)
					states.Add(new(property.Identifier.ValueText));
			}

			foreach (FieldDeclarationSyntax field in tds.ChildNodes().OfType<FieldDeclarationSyntax>())
			{
				if (field.GetAttributeFromMember(_childAttributeTypeName.Type) != null)
				{
					foreach (VariableDeclaratorSyntax variable in field.Declaration.Variables)
						children.Add(new(variable.Identifier.ValueText));
				}
				else if (field.GetAttributeFromMember(_interpolationAttributeTypeName.Type) != null)
				{
					foreach (VariableDeclaratorSyntax variable in field.Declaration.Variables)
						states.Add(new(variable.Identifier.ValueText));
				}
			}

			gameObjects.Add(new(gameObjectTypeName, states, children));
		}

		return gameObjects;
	}
}
