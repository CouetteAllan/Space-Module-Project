using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class Utils
    {
        //Return random vector 2 dir in a given radius
        public static Camera Camera
        {
            get {
                if (_cam == null)
                    _cam = Camera.main;
                return _cam;
            }
        }
        private static Camera _cam = null;

        /// <summary>
        /// Return true if a random number is under the parameter
        /// </summary>
        /// <param name="chance"> the chances to succeed (must be between 0.0 and 1.0 inclusive </param>
        /// <returns></returns>
        public static bool RollChance(float chance)
        {
            chance = Mathf.Clamp01(chance);
            float randomNumber = Random.value;
            return randomNumber < chance;
        }
    }

}
