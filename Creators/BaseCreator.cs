using UnityEngine;
using System.Collections;

namespace BaseGameLogic
{
    /// <summary>
    /// Base creator.
    /// Each creator should inherit from this class.
    /// </summary>
    public abstract class BaseCreator : ScriptableObject 
    {
        [SerializeField]
        protected string productName = "";
        public string ProductName { get { return this.productName; } }
    }
}