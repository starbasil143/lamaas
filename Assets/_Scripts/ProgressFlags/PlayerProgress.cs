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
      {"deerDefeated", false},
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
         bool flagReached = false;
         foreach (string flag in new List<string>(progressFlags.Keys))
         {
            if (!flagReached)
            {
               progressFlags[flag] = true;
               if (flag == setAllTrueUpToPoint)
               {
                  flagReached = true;
               }
            }
         }
      }
      foreach (string tag in setAutomatically)
      {
         progressFlags[tag] = true;
      }
   }

}
