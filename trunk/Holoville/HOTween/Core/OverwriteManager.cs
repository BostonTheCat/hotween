// 
// OverwriteManager.cs
//  
// Author: Daniele Giardini
// 
// Copyright (c) 2012 Daniele Giardini - Holoville - http://www.holoville.com
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins.Core;

namespace Holoville.HOTween.Core
{
	/// <summary>
	/// Manager used for automatic control of eventual overwriting of tweens.
	/// </summary>
	internal class OverwriteManager
	{
		// VARS ///////////////////////////////////////////////////
		
		/// <summary>
		/// List of currently running Tweeners
		/// (meaning all Tweeners whose OnStart has been called, paused or not).
		/// </summary>
		private		List<Tweener>			runningTweens;
		
		
		// ***********************************************************************************
		// CONSTRUCTOR
		// ***********************************************************************************

		public OverwriteManager()
		{
			runningTweens = new List<Tweener>();
		}
		
		// ===================================================================================
		// METHODS ---------------------------------------------------------------------------
		
		public void AddTween( Tweener p_tween )
		{
			// Check running tweens for eventual overwrite.
			List<ABSTweenPlugin> addPlugs = p_tween.plugins;
			Tweener tw;
			ABSTweenPlugin addPlug;
			ABSTweenPlugin plug;
			string t0;
			string t1;
			for ( int i = runningTweens.Count - 1; i > -1; --i ) {
				tw = runningTweens[i];
				if ( tw.target == p_tween.target ) {
					// Check internal plugins.
					for ( int n = 0; n < addPlugs.Count; ++n ) {
						addPlug = addPlugs[n];
						for ( int c = tw.plugins.Count - 1; c > -1; --c ) {
							plug = tw.plugins[c];
							if ( addPlug.pluginId == -1 || plug.pluginId == -1 || plug.pluginId == addPlug.pluginId && plug.propName == addPlug.propName ) {
								// Overwrite old plugin.
								tw.plugins.RemoveAt( c );
								if ( HOTween.isEditor && HOTween.warningLevel == WarningLevel.Verbose ) {
									t0 = addPlug.GetType().ToString();
									t0 = t0.Substring( t0.LastIndexOf( "." ) + 1 );
									t1 = plug.GetType().ToString();
									t1 = t1.Substring( t1.LastIndexOf( "." ) + 1 );
									TweenWarning.Log( t0 + " is overwriting " + t1 + " for " + tw.target + "." + plug.propName );
								}
								// Check if whole tween needs to be removed.
								if ( tw.plugins.Count == 0 ) {
									Debug.Log( "Killing TWEEN" );
									if ( tw.isSequenced )	tw.contSequence.Remove( tw );
									runningTweens.RemoveAt( i );
									tw.Kill();
									goto NEXT_TWEEN;
								}
							}
						}
					}
				NEXT_TWEEN:
					continue;
				}
			}
			runningTweens.Add( p_tween );
		}
		
		public void RemoveTween( Tweener p_tween )
		{
			for ( int i = 0; i < runningTweens.Count; ++i ) {
				if ( runningTweens[i] == p_tween ) {
					runningTweens.RemoveAt( i );
					break;
				}
			}
		}
		
		public void RemoveAllTweens()
		{
			runningTweens = new List<Tweener>();
		}
	}
}
