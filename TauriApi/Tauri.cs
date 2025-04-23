using TauriApi.Modules;

namespace TauriApi;

public class Tauri
{
    public Tauri(
        TauriApp tauriApp, 
        TauriCore tauriCore, 
        TauriEvent tauriEvent, 
        TauriWindow tauriWindow)
    {
        App = tauriApp;
        Core = tauriCore;
        Event = tauriEvent;
        Window = tauriWindow;
    }

    public TauriApp App { get;  }
    public TauriCore Core { get;  }
    public TauriEvent Event { get;  }
    public TauriWindow Window { get; }
}