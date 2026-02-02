using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Common.Saves
{
	public sealed class PlayerPrefsSavesWriter : ISavesWriter
	{
		public void DeleteSave(string key)
		{
			PlayerPrefs.DeleteKey(key);
			PlayerPrefs.Save();
		}

		public bool HasSave(string key)
			=> PlayerPrefs.HasKey(key);

		public void DeleteAllSaves()
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}
		
		public void WriteSave<T>(string key, T value)
		{
			switch (value)
			{
				case string stringValue:
					PlayerPrefs.SetString(key, stringValue);
					break;
                
				case int intValue:
					PlayerPrefs.SetInt(key, intValue);
					break;
                
				case float floatValue:
					PlayerPrefs.SetFloat(key, floatValue);
					break;
                
				case bool boolValue:
					PlayerPrefs.SetInt(key, boolValue ? 1 : 0);
					break;
                
				case long longValue:
					PlayerPrefs.SetString(key, longValue.ToString());
					break;
                
				case double doubleValue:
					PlayerPrefs.SetFloat(key, (float)doubleValue);
					break;
                
				case Enum enumValue:
					PlayerPrefs.SetString(key, enumValue.ToString());
					break;
            
				case Type { IsClass: true }:
						PlayerPrefs.SetString(key, JsonUtility.ToJson(value)); 
						break;
				
				default:	
					PlayerPrefs.SetString(key, JsonConvert.SerializeObject(value));
					break;
			}
        
			PlayerPrefs.Save();
		}
		
		public T ReadSave<T>(string key, T defaultValue = default)
		{
			if (!PlayerPrefs.HasKey(key))
				return defaultValue;

			var type = typeof(T);
    
			if (type == typeof(string))
				return (T)(object)PlayerPrefs.GetString(key, defaultValue as string);
    
			if (type == typeof(int))
				return (T)(object)PlayerPrefs.GetInt(key, defaultValue is int defInt ? defInt : 0);
    
			if (type == typeof(float))
				return (T)(object)PlayerPrefs.GetFloat(key, defaultValue is float defFloat ? defFloat : 0f);
    
			if (type == typeof(bool))
			{
				var defBool = defaultValue is true;
				var intValue = PlayerPrefs.GetInt(key, defBool ? 1 : 0);
				return (T)(object)(intValue == 1);
			}
    
			if (type == typeof(long))
			{
				var stringValue = PlayerPrefs.GetString(key);
				return long.TryParse(stringValue, out var longValue) 
					? (T)(object)longValue 
					: defaultValue;
			}
    
			if (type == typeof(double))
			{
				var floatValue = PlayerPrefs.GetFloat(key, 0f);
				return (T)(object)(double)floatValue;
			}
    
			if (type.IsEnum)
			{
				var stringValue = PlayerPrefs.GetString(key);
				return Enum.TryParse(type, stringValue, out var enumValue) 
					? (T)enumValue 
					: defaultValue;
			}
    
			try
			{
				var json = PlayerPrefs.GetString(key);
				if (!string.IsNullOrEmpty(json))
					return JsonConvert.DeserializeObject<T>(json);
			}
			catch (Exception ex)
			{
				Debug.LogError($"Failed to read save '{key}': {ex.Message}");
			}
    
			return defaultValue;
		}
	}
}