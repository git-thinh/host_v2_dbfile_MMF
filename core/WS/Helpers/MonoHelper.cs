using System;

namespace core.websocket.Helpers
{
  public static class MonoHelper
  {
    public static bool IsRunningOnMono ()
    {
      return Type.GetType ("Mono.Runtime") != null;
    }
  }
}

