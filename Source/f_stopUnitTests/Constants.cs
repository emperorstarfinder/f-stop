﻿// Constants.cs
//
// Author:
//       Ricky Curtice <ricky@rwcproductions.com>
//
// Copyright (c) 2017 Richard Curtice
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

using System;
using System.IO;
using NUnit.Framework;

namespace f_stopUnitTests {
	internal static class Constants {
		public static readonly string EXECUTABLE_DIRECTORY;

		public static readonly string LOG_CONFIG_PATH;

		public static readonly string TEST_CACHE_PATH;

		public static readonly string TEST_WRITE_CACHE_PATH;

		public static readonly TimeSpan SERVICE_NC_LIFETIME = TimeSpan.FromSeconds(2);

		static Constants() {
			EXECUTABLE_DIRECTORY = TestContext.CurrentContext.TestDirectory;
			LOG_CONFIG_PATH = Path.Combine(EXECUTABLE_DIRECTORY, "UnitTests.F_Stop.config");
			TEST_CACHE_PATH = Path.Combine(EXECUTABLE_DIRECTORY, "UnitTestsCache");
			TEST_WRITE_CACHE_PATH = Path.Combine(EXECUTABLE_DIRECTORY, "UnitTestsCache.wcache");
		}
	}
}