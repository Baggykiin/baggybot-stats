namespace baggybot_stats.ApiModel
{
	class Error
	{
		public int ErrorCode { get; }
		public string ErrorMessage { get; }
		private Error(int errorCode, string errorMessage)
		{
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}

		public static Error AuthenticationRequired => new Error(101, "A request token must be supplied.");
		public static Error InvalidRequestToken => new Error(102, "The supplied request token is invalid.");
	}
}
