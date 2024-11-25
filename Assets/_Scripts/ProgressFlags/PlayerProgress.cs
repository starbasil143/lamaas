using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
   // This script is basically a checklist for what story points have occured. Other objects will refer to it
   // to see if they should exist.

   public List<string> setAutomatically;

   public Dictionary<string, bool> progressFlags = new Dictionary<string, bool>()
   {
      {"openingScene", false},
      {"malloryVisit", false},
      {"forestVisit", false},
   };

   private void Awake()
   {
      foreach (string tag in setAutomatically)
      {
         progressFlags[tag] = true;
      }
   }

}
