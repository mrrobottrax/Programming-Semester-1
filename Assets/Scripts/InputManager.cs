public static class InputManager
{
	public static DefaultControls Controls { get; private set; }

	public static void Awake()
	{
		Controls = new DefaultControls();
		Controls.Enable();
	}

	public static void Destroy()
	{
		Controls.Dispose();
	}
}
