using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
   // This script is basically a checklist for what story points have occured. Other objects will refer to it
   // to see if they should exist.

   public List<string> setAutomatically;
   public bool setAllTrue;
   public string setAllTrueUpToPoint;

   public Dictionary<string, bool> progressFlags = new Dictionary<string, bool>()
   {
      {"openingScene", false},
      {"malloryVisit", false},
      {"deerCutscene", false},
      {"forestVisit1", false},
   };

   private void Awake()
   {
      if (setAllTrue)
      {
         foreach (string flag in progressFlags.Keys)
         {
            progressFlags[flag] = true;
         }
      }
      if (setAllTrueUpToPoint != "")
      {
         foreach (string flag in progressFlags.Keys)
         {
            progressFlags[flag] = true;
            if (flag == setAllTrueUpToPoint)
            {
               break;
            }
         }
      }
      foreach (string tag in setAutomatically)
      {
         progressFlags[tag] = true;
      }
   }

}
