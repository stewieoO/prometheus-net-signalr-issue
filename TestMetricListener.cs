using System.Diagnostics.Metrics;

namespace PrometheusNetSignalRIssue;

public static class TestMetricListener
{
  public static void Listen()
  {
    var l = new MeterListener();
    l.InstrumentPublished = (instrument, _) =>
    {
      if (instrument.Name is "signalr.server.active_connections")
      {
        l.EnableMeasurementEvents(instrument);
        Console.WriteLine(instrument.Name);
      }
    };
    l.SetMeasurementEventCallback<byte>(OnMeasurementRecorded);
    l.SetMeasurementEventCallback<short>(OnMeasurementRecorded);
    l.SetMeasurementEventCallback<int>(OnMeasurementRecorded);
    l.SetMeasurementEventCallback<long>(OnMeasurementRecorded);
    l.SetMeasurementEventCallback<float>(OnMeasurementRecorded);
    l.SetMeasurementEventCallback<double>(OnMeasurementRecorded);
    l.SetMeasurementEventCallback<decimal>(OnMeasurementRecorded);
    l.RecordObservableInstruments();
    l.Start();
  }
  
  private static void OnMeasurementRecorded<T>(Instrument instrument, T measurement,
    ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state) where T: struct
  {
    Console.WriteLine(instrument.Name + " " + measurement);
  }
}