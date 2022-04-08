int entityId = 1;

List<IEvent> events = new();
events.Add(new HitEvent(353333333, 353333333, 353333333));
events.Add(new SquidSpawnEvent(entityId++, SquidType.Squid2, 1, new(35.151543f, 8.5f, -51.072197f), new(5, 5, 5), -549.1759f));
events.Add(new InitialInputsEvent(0, 0, 0, 0, 0, 0, 0, 0, 0, 0.05f));

for (int i = 0; i < 180; i++)
{
	if (i % 20 == 0)
		events.Add(new SquidSpawnEvent(entityId++, SquidType.Squid2, 2, new(35.151543f, 8.5f, -51.072197f), new(5, 5, 5), -549.1759f));

	//if (i % 20 == 0)
	events.Add(new DaggerSpawnEvent(entityId++, 3, new(5, 14, 5), Int16Mat3x3.Identity, 3, 1));

	events.Add(new InputsEvent(0, 0, 1, 0, (byte)(i % 60 == 0 ? 1 : 0), 1, 0, 1, 0));
}

events.Add(new EndEvent());

ReplayBinary two = new(1, 2, events.Count(e => e is InputsEvent) / 60f, 0, 0, 0, 0, 0, 0, 999999, "PEP", File.ReadAllBytes(Path.Combine("Resources", "Squid2")), ReplayEventsParser.CompileEvents(events));
File.WriteAllBytes(@"C:\Users\NOAH\AppData\Roaming\DevilDaggers\replays\TEST.ddreplay", two.Compile());
