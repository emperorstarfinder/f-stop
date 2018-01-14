﻿// TestRange.cs
//
// Author:
//       Ricky Curtice <ricky@rwcproductions.com>
//
// Copyright (c) 2018 Richard Curtice
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

using NUnit.Framework;
using System;
using LibF_Stop;
using System.Collections.Generic;
using System.Linq;

namespace f_stopUnitTests {
	[TestFixture]
	public class TestRange {
		#region Ctor
		/*
			null null OK  null    
			null -1   OK  "-1"    
			null 1    BAD "-1"    
			null 0    BAD "-0"    
			-1 null   BAD "-1-"   
			0 null    OK  "0-"    
			1 null    OK  "1-"    

			-2 -1     BAD "-2--2" 
			-1 -2     BAD "-1--2" 
			-1 -1     BAD "-1--1" 
			-1 0      BAD "-1-0"  
			-1 1      BAD "-1-1"  
			0 -1      BAD "0--1"  
			0 0       OK  "0-0"   
			0 1       OK  "0-1"   
			1 -1      BAD "1--1"  
			1 0       BAD "1-0"   
			1 1       OK  "1-1"   
			1 2       OK  "1-2"   
			2 1       BAD "2-1"   
		*/
		[Test]
		public void TestRange_Ctor_Null_Null_DoesNotThrow() {
			Assert.DoesNotThrow(() => new Range(null, null));
		}


		[Test]
		public void TestRange_Ctor_Null_dash1_DoesNotThrow() {
			Assert.DoesNotThrow(() => new Range(null, -1));
		}

		[Test]
		public void TestRange_Ctor_Null_0_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(null, 0));
		}

		[Test]
		public void TestRange_Ctor_Null_1_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(null, 1));
		}


		[Test]
		public void TestRange_Ctor_dash1_Null_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(-1, null));
		}

		[Test]
		public void TestRange_Ctor_0_Null_DoesNotThrow() {
			Assert.DoesNotThrow(() => new Range(0, null));
		}

		[Test]
		public void TestRange_Ctor_1_Null_DoesNotThrow() {
			Assert.DoesNotThrow(() => new Range(1, null));
		}


		[Test]
		public void TestRange_Ctor_dash2_dash1_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(-2, -1));
		}

		[Test]
		public void TestRange_Ctor_dash1_dash2_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(-1, -2));
		}

		[Test]
		public void TestRange_Ctor_dash1_dash1_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(-1, -1));
		}

		[Test]
		public void TestRange_Ctor_dash1_0_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(-1, 0));
		}

		[Test]
		public void TestRange_Ctor_dash1_1_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(-1, 1));
		}


		[Test]
		public void TestRange_Ctor_0_dash1_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(0, -1));
		}

		[Test]
		public void TestRange_Ctor_0_0_DoesNotThrow() {
			Assert.DoesNotThrow(() => new Range(0, 0));
		}

		[Test]
		public void TestRange_Ctor_0_1_DoesNotThrow() {
			Assert.DoesNotThrow(() => new Range(0, 1));
		}


		[Test]
		public void TestRange_Ctor_1_dash1_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(1, -1));
		}

		[Test]
		public void TestRange_Ctor_1_0_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(1, 0));
		}

		[Test]
		public void TestRange_Ctor_1_1_DoesNotThrow() {
			Assert.DoesNotThrow(() => new Range(1, 1));
		}

		[Test]
		public void TestRange_Ctor_1_2_DoesNotThrow() {
			Assert.DoesNotThrow(() => new Range(1, 2));
		}


		[Test]
		public void TestRange_Ctor_2_1_ArgumentOutOfRangeException() {
			Assert.Throws<ArgumentOutOfRangeException>(() => new Range(2, 1));
		}

		#endregion


		#region GetRange

		[Test]
		public void TestRange_GetRange_Null_Null_Null() {
			var list = new List<byte> { 0, 1, 2, 3, 4, 5 };
			Assert.IsNull(new Range(null, null).GetRange(list));
		}

		[Test]
		public void TestRange_GetRange_NullList_Null() {
			Assert.IsNull(new Range(0, 1).GetRange((List<byte>)null));
		}


		[Test]
		public void TestRange_GetRange_Null_dash2_Correct() {
			var list = new List<byte> { 0, 1, 2, 3, 4, 5 };
			Assert.AreEqual(new List<byte> { 4, 5 }, new Range(null, -2).GetRange(list));
		}

		[Test]
		public void TestRange_GetRange_Null_dash1_Correct() {
			var list = new List<byte> { 0, 1, 2, 3, 4, 5 };
			Assert.AreEqual(new List<byte> { 5 }, new Range(null, -1).GetRange(list));
		}

		[Test]
		public void TestRange_GetRange_0_Null_Correct() {
			var list = new List<byte> {0, 1, 2, 3, 4, 5};
			Assert.AreEqual(list, new Range(0, null).GetRange(list));
		}

		[Test]
		public void TestRange_GetRange_1_Null_Correct() {
			var list = new List<byte> {0, 1, 2, 3, 4, 5};
			Assert.AreEqual(list.Skip(1), new Range(1, null).GetRange(list));
		}


		[Test]
		public void TestRange_GetRange_0_0_Correct() {
			var list = new List<byte> {0, 1, 2, 3, 4, 5};
			Assert.AreEqual(list.Take(1), new Range(0, 0).GetRange(list));
		}

		[Test]
		public void TestRange_GetRange_0_1_Correct() {
			var list = new List<byte> {0, 1, 2, 3, 4, 5};
			Assert.AreEqual(list.Take(2), new Range(0, 1).GetRange(list));
		}


		[Test]
		public void TestRange_GetRange_1_1_Correct() {
			var list = new List<byte> {0, 1, 2, 3, 4, 5};
			Assert.AreEqual(list.Skip(1).Take(1), new Range(1, 1).GetRange(list));
		}

		[Test]
		public void TestRange_GetRange_1_2_Correct() {
			var list = new List<byte> {0, 1, 2, 3, 4, 5};
			Assert.AreEqual(list.Skip(1).Take(2), new Range(1, 2).GetRange(list));
		}

		#endregion


		#region ToString

		[Test]
		public void TestRange_ToString_Null_Null_Correct() {
			Assert.IsNull(new Range(null, null).ToString());
		}


		[Test]
		public void TestRange_ToString_Null_dash1_Correct() {
			Assert.AreEqual("-1", new Range(null, -1).ToString());
		}

		[Test]
		public void TestRange_ToString_0_Null_Correct() {
			Assert.AreEqual("0-", new Range(0, null).ToString());
		}

		[Test]
		public void TestRange_ToString_1_Null_Correct() {
			Assert.AreEqual("1-", new Range(1, null).ToString());
		}


		[Test]
		public void TestRange_ToString_0_0_Correct() {
			Assert.AreEqual("0-0", new Range(0, 0).ToString());
		}

		[Test]
		public void TestRange_ToString_0_1_Correct() {
			Assert.AreEqual("0-1", new Range(0, 1).ToString());
		}


		[Test]
		public void TestRange_ToString_1_1_Correct() {
			Assert.AreEqual("1-1", new Range(1, 1).ToString());
		}

		[Test]
		public void TestRange_ToString_1_2_Correct() {
			Assert.AreEqual("1-2", new Range(1, 2).ToString());
		}

		#endregion
	}
}