namespace WarThunderStreamDeckPlugin.Telemetry;

// Result of polling War Thunder telemetry for one action's display.
//   State: which Stream Deck state (0/1) to show.
//   Percent: current value 0..100 to render on the button title; null = no title text.
public readonly record struct TelemetryReading(int State, double? Percent);
