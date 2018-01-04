﻿// F_StopRouter.cs
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
using System.Collections.Generic;
using System.Text;
using Nancy;

namespace LibF_Stop {
	public class F_StopRouter : NancyModule {
		private static readonly log4net.ILog LOG = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private static CapAdministration _capAdmin = new CapAdministration(); 

		public F_StopRouter() : base("/CAPS/HTT") {
			Get["/TEST"] = _ => {
				LOG.Debug($"Test called by {Request.UserHostAddress}");

				return Response.AsText("OK");
			};

			Get["/ADDCAP/{adminToken}/{capId:guid}/{bandwidth?}"] = _ => {
				if (_.bandwidth != null && (int)_.bandwidth < 0) {
					LOG.Warn($"Invalid bandwidth spec from {Request.UserHostAddress} on cap {_.capId}: bandwidth cannot be negative ({_.bandwidth})");
					return StockReply.BadRequest;
				}

				uint bandwidth = 0;
				if (_.bandwidth != null) {
					bandwidth = _.bandwidth;
				}

				try {
					var result = _capAdmin.AddCap(_.adminToken, _.capId, bandwidth);

					return result ? StockReply.Ok : StockReply.BadRequest;
				}
				catch (InvalidAdminTokenException) {
					LOG.Warn($"Invalid admin token from {Request.UserHostAddress}");
				}

				return StockReply.BadRequest;
			};

			Get["/REMCAP/{adminToken}/{capId:guid}"] = _ => {
				try {
					var result = _capAdmin.RemoveCap(_.adminToken, _.capId);

					return result ? StockReply.Ok : StockReply.BadRequest;
				}
				catch (InvalidAdminTokenException) {
					LOG.Warn($"Invalid admin token from {Request.UserHostAddress}");
				}

				return StockReply.BadRequest;
			};

			Get["/PAUSE/{adminToken}/{capId:guid}"] = _ => {
				try {
					var result = _capAdmin.PauseCap(_.adminToken, _.capId);

					return result ? StockReply.Ok : StockReply.BadRequest;
				}
				catch (InvalidAdminTokenException) {
					LOG.Warn($"Invalid admin token from {Request.UserHostAddress}");
				}

				return StockReply.BadRequest;
			};

			Get["/RESUME/{adminToken}/{capId:guid}"] = _ => {
				try {
					var result = _capAdmin.ResumeCap(_.adminToken, _.capId);

					return result ? StockReply.Ok : StockReply.BadRequest;
				}
				catch (InvalidAdminTokenException) {
					LOG.Warn($"Invalid admin token from {Request.UserHostAddress}");
				}

				return StockReply.BadRequest;
			};

			Get["/LIMIT/{adminToken}/{capId:guid}/{bandwidth?}"] = _ => {
				if (_.bandwidth != null && (int)_.bandwidth < 0) {
					LOG.Warn($"Invalid bandwidth spec from {Request.UserHostAddress} on cap {_.capId}: bandwidth cannot be negative ({_.bandwidth})");
					return StockReply.BadRequest;
				}

				uint bandwidth = 0;
				if (_.bandwidth != null) {
					bandwidth = _.bandwidth;
				}

				try {
					var result = _capAdmin.LimitCap(_.adminToken, _.capId, bandwidth);

					return result ? StockReply.Ok : StockReply.BadRequest;
				}
				catch (InvalidAdminTokenException) {
					LOG.Warn($"Invalid admin token from {Request.UserHostAddress}");
				}

				return StockReply.BadRequest;
			};

			Get["/{capId:guid}"] = _ => {
				// TODO: split along texture/mesh thoughts
				var textureId = (Guid?)Request.Query["texture_id"];
				var meshId = (Guid?)Request.Query["mesh_id"];

				if (textureId == null && meshId == null) {
					LOG.Warn($"Bad request for asset from {Request.UserHostAddress}: mesh_id nor texture_id supplied");
					return StockReply.BadRequest;
				}

				if (textureId != null && meshId != null) {
					// Difference from Aperture: Aperture continues and only uses the texture_id when both are specc'd.
					LOG.Warn($"Bad request for asset from {Request.UserHostAddress}: both mesh_id and texture_id supplied");
					return StockReply.BadRequest;
				}

				try {
					//var result = capAdmin.LimitCap(_.adminToken, _.capId, bandwidth);

					return (Response)"";
				}
				catch (Exception e) {
					return (Response)"";
				}
			};
		}

		private static class StockReply {
			public static Response Ok = new Response {
				ContentType = "text/html",
				StatusCode = HttpStatusCode.OK,
				//Contents = stream => (new System.IO.StreamWriter(stream) { AutoFlush = true }).Write(@""),
				Headers = new Dictionary<string, string> {
					{"Content-Length", "0"},
				}
			};

			public static Response BadRequest = new Response {
				ContentType = "text/html",
				StatusCode = HttpStatusCode.BadRequest,
				Contents = stream => (new System.IO.StreamWriter(stream) { AutoFlush = true }).Write(@"<html>
<head><title>Bad Request</title></head>
<body><h1>400 Bad Request</h1></body>
</html>"),
			};

			public static Response NotFound = new Response {
				ContentType = "text/html",
				StatusCode = HttpStatusCode.NotFound,
				Contents = stream => (new System.IO.StreamWriter(stream) { AutoFlush = true }).Write(@"<html>
<head><title>Not Found</title></head>
<body><h1>404 Not Found</h1></body>
</html>"),
			};

			public static Response ServiceUnavailable = new Response {
				ContentType = "text/html",
				StatusCode = HttpStatusCode.ServiceUnavailable,
				Contents = stream => (new System.IO.StreamWriter(stream) { AutoFlush = true }).Write(@"<html>
<head><title>Service Unavailable</title></head>
<body><h1>503 Service Unavailable</h1></body>
</html>"),
			};

			public static Response InternalServerError = new Response {
				ContentType = "text/html",
				StatusCode = HttpStatusCode.InternalServerError,
				Contents = stream => (new System.IO.StreamWriter(stream) { AutoFlush = true }).Write(@"<html>
<head><title>Internal Server Error</title></head>
<body><h1>500 Internal Server Error</h1></body>
</html>"),
			};

			public static Response RangeError = new Response {
				ContentType = "text/html",
				StatusCode = HttpStatusCode.RequestedRangeNotSatisfiable,
				Contents = stream => (new System.IO.StreamWriter(stream) { AutoFlush = true }).Write(@"<html>
<head><title>Requested Range not satisfiable</title></head>
<body><h1>416 Requested Range not satisfiable</h1></body>
</html>"),
			};
		}
	}
}
