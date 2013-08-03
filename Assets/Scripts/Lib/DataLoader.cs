using UnityEngine;
using System.Collections;

public class DataLoader {
	
	private string		dataFile;
	private string[,]	dataBody;
	private int			ballCount;
	
	public bool LoadData(string dataFile)
	{
		this.dataFile = dataFile;
		Debug.Log(this.dataFile);
		TextAsset textAsset = (TextAsset)Resources.Load(this.dataFile, typeof(TextAsset));
		if (textAsset == null)
			return false;

		dataBody = CSVReader.SplitCsvGrid(textAsset.text);
		CSVReader.DebugOutputGrid(dataBody);
		
		return true;
	}
	
	public int getLineCnt()
	{
		return dataBody.GetUpperBound(1) - 1;
	}
	
	public int getItemCnt()
	{
		return dataBody.GetUpperBound(0) - 1;
	}
	
	public string GetGrid(int line, int pos)
	{
		if (line < 0 || line >= dataBody.GetUpperBound(1) - 1)
			return "";
		
		if (pos < 0 || pos >= dataBody.GetUpperBound(0) - 1)
			return "";
		
		return dataBody[pos, line];
	}
	
	public float GetGridFloat(int line, int pos, float default_value)
	{
		string float_string = GetGrid(line, pos);
		if (0 == float_string.Length)
			return default_value;
		
		float float_result = default_value;
		if (float.TryParse(float_string, out float_result))
			return float_result;
		return default_value;
	}
	
	public int GetGridInt(int line, int pos, int default_value)
	{
		string int_string = GetGrid(line, pos);
		if (0 == int_string.Length)
			return default_value;
		
		int int_result = default_value;
		if (int.TryParse(int_string, out int_result))
			return int_result;
		return default_value;
	}
	
	public bool GetGridBool(int line, int pos, bool default_value)
	{
		string bool_string = GetGrid(line, pos);
		if (0 == bool_string.Length)
			return default_value;
		
		bool bool_result = default_value;
		if (bool.TryParse(bool_string, out bool_result))
			return bool_result;
		return default_value;
	}
	
}
