[System.Serializable]
public class Msg
{
	public static string language="ru_RU";
	public static string get(string key,string lang)
	{
		return "";
	}
	public static string get(string key)
	{
		return get(key,language);
	}



	private string key;

	public Msg(string key)
	{
		this.key=key;
	}

	public static implicit operator string(Msg msg)
	{
		return Msg.get(msg.key);
	}
}
