using System;
using System.Collections.Generic;
using System.Text;

namespace FileProcessingPublisher
{
  public class BusOptions
  {
    public string FullyQualifiedNamespace { get; set; } = null!;
    public string TopicName { get; set; } = null!;
    public int MaxRetries { get; set; } = 3;
  }
}
