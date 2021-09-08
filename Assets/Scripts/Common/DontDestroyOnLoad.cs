using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
	/// <summary>
	/// 
	/// </summary>
	public class DontDestroyOnLoad : MonoBehaviour 
	{
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}