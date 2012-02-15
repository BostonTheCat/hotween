// 
// PlugVector4.cs
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
using System;

namespace Holoville.HOTween.Plugins.Core
{
	/// <summary>
	/// Default plugin for the tweening of Vector4 objects.
	/// </summary>
	public class PlugVector4 : ABSTweenPlugin
	{
		// VARS ///////////////////////////////////////////////////
		
		static internal	Type[]			validPropTypes = { typeof(Vector4) };
		static internal	Type[]			validValueTypes = { typeof(Vector4) };
		
		private		Vector4				typedStartVal;
		private		Vector4				typedEndVal;
		private		Vector4				changeVal;
		
		// GETS/SETS //////////////////////////////////////////////
		
		/// <summary>
		/// Gets the untyped start value,
		/// sets both the untyped and the typed start value.
		/// </summary>
		override protected	object		startVal {
			get { return _startVal; }
			set {
				if ( tweenObj.isFrom && isRelative ) {
					_startVal = typedStartVal = typedEndVal + (Vector4)value;
				} else {
					_startVal = typedStartVal = (Vector4)value;
				}
			}
		}
		
		/// <summary>
		/// Gets the untyped end value,
		/// sets both the untyped and the typed end value.
		/// </summary>
		override protected	object		endVal {
			get { return _endVal; }
			set { _endVal = typedEndVal = (Vector4)value; }
		}
		
		
		// ***********************************************************************************
		// CONSTRUCTOR
		// ***********************************************************************************
		
		/// <summary>
		/// Creates a new instance of this plugin using the main ease type.
		/// </summary>
		/// <param name="p_endVal">
		/// The <see cref="Vector4"/> value to tween to.
		/// </param>
		public PlugVector4( Vector4 p_endVal ) : base( p_endVal, false ) {}
		/// <summary>
		/// Creates a new instance of this plugin.
		/// </summary>
		/// <param name="p_endVal">
		/// The <see cref="Vector4"/> value to tween to.
		/// </param>
		/// <param name="p_easeType">
		/// The <see cref="EaseType"/> to use.
		/// </param>
		public PlugVector4( Vector4 p_endVal, EaseType p_easeType ) : base( p_endVal, p_easeType, false ) {}
		/// <summary>
		/// Creates a new instance of this plugin using the main ease type.
		/// </summary>
		/// <param name="p_endVal">
		/// The <see cref="Vector4"/> value to tween to.
		/// </param>
		/// <param name="p_isRelative">
		/// If <c>true</c>, the given end value is considered relative instead than absolute.
		/// </param>
		public PlugVector4( Vector4 p_endVal, bool p_isRelative ) : base( p_endVal, p_isRelative ) {}
		/// <summary>
		/// Creates a new instance of this plugin.
		/// </summary>
		/// <param name="p_endVal">
		/// The <see cref="Vector4"/> value to tween to.
		/// </param>
		/// <param name="p_easeType">
		/// The <see cref="EaseType"/> to use.
		/// </param>
		/// <param name="p_isRelative">
		/// If <c>true</c>, the given end value is considered relative instead than absolute.
		/// </param>
		public PlugVector4( Vector4 p_endVal, EaseType p_easeType, bool p_isRelative ) : base( p_endVal, p_easeType, p_isRelative ) {}
		
		// ===================================================================================
		// METHODS ---------------------------------------------------------------------------
		
		/// <summary>
		/// Returns the speed-based duration based on the given speed x second.
		/// </summary>
		override protected float GetSpeedBasedDuration( float p_speed )
		{
			float speedDur = changeVal.magnitude / p_speed;
			if ( speedDur < 0 )		speedDur = -speedDur;
			return speedDur;
		}
		
		/// <summary>
		/// Sets the typed changeVal based on the current startVal and endVal.
		/// </summary>
		override protected void SetChangeVal()
		{
			if ( isRelative && !tweenObj.isFrom )
				changeVal = typedEndVal;
			else
				changeVal = new Vector4( typedEndVal.x - typedStartVal.x, typedEndVal.y - typedStartVal.y, typedEndVal.z - typedStartVal.z, typedEndVal.w - typedStartVal.w );
		}
		
		/// <summary>
		/// Updates the tween.
		/// </summary>
		/// <param name="p_totElapsed">
		/// The total elapsed time since startup.
		/// </param>
		override protected void DoUpdate ( float p_totElapsed )
		{
			float x = ease( p_totElapsed, typedStartVal.x, changeVal.x, _duration );
			float y = ease( p_totElapsed, typedStartVal.y, changeVal.y, _duration );
			float z = ease( p_totElapsed, typedStartVal.z, changeVal.z, _duration );
			float w = ease( p_totElapsed, typedStartVal.w, changeVal.w, _duration );
			
			SetValue( new Vector4( x, y, z, w ) );
		}
	}
}

