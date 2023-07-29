namespace LastKill
{
	public interface IDebug
	{
		public bool ShowDebugLog { get; set; }
		public bool ShowDebugCrouch { get; set; }
		public void DebugCrouch(float a, float b);
		public bool ShowDebugCrawl { get; set; }
		public bool ShowDebugRoll { get; set; }
		public bool ShowDebugShortClimb { get; set; }

	}
}
