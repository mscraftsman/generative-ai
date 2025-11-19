/*
 * Copyright 2024-2025 Jochen Kirstätter
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The <see cref="Schema"/> object allows the definition of input and output data types. These types can be objects, but also primitives and arrays. Represents a select subset of an [OpenAPI 3.0 schema object](https://spec.openapis.org/oas/v3.0.3#schema).
	/// </summary>
	public partial class Schema
	{
		/// <summary>
		/// Optional. The value should be validated against any (one or more) of the subschemas in the list.
		/// </summary>
		public List<Schema>? AnyOf { get; set; }
		/// <summary>
		/// Optional. Default value of the field. Per JSON Schema, this field is intended for documentation generators and doesn't affect validation. Thus it's included here and ignored so that developers who send schemas with a <see cref="default"/> field don't get unknown-field errors.
		/// </summary>
		public object? Default { get; set; }
		/// <summary>
		/// Optional. A brief description of the parameter. This could contain examples of use. Parameter description may be formatted as Markdown.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// Optional. Possible values of the element of Type.STRING with enum format. For example we can define an Enum Direction as : {type:STRING, format:enum, enum:["EAST", NORTH", "SOUTH", "WEST"]}
		/// </summary>
		public List<string>? Enum { get; set; }
		/// <summary>
		/// Optional. Example of the object. Will only populated when the object is the root.
		/// </summary>
		public object? Example { get; set; }
		/// <summary>
		/// Optional. The format of the data. Any value is allowed, but most do not trigger any special functionality.
		/// </summary>
		public string? Format { get; set; }
		/// <summary>
		/// Optional. Schema of the elements of Type.ARRAY.
		/// </summary>
		public Schema? Items { get; set; }
		/// <summary>
		/// Optional. Maximum number of the elements for Type.ARRAY.
		/// </summary>
		public long? MaxItems { get; set; }
		/// <summary>
		/// Optional. Maximum length of the Type.STRING
		/// </summary>
		public long? MaxLength { get; set; }
		/// <summary>
		/// Optional. Maximum number of the properties for Type.OBJECT.
		/// </summary>
		public long? MaxProperties { get; set; }
		/// <summary>
		/// Optional. Maximum value of the Type.INTEGER and Type.NUMBER
		/// </summary>
		public double? Maximum { get; set; }
		/// <summary>
		/// Optional. Minimum number of the elements for Type.ARRAY.
		/// </summary>
		public long? MinItems { get; set; }
		/// <summary>
		/// Optional. SCHEMA FIELDS FOR TYPE STRING Minimum length of the Type.STRING
		/// </summary>
		public long? MinLength { get; set; }
		/// <summary>
		/// Optional. Minimum number of the properties for Type.OBJECT.
		/// </summary>
		public long? MinProperties { get; set; }
		/// <summary>
		/// Optional. SCHEMA FIELDS FOR TYPE INTEGER and NUMBER Minimum value of the Type.INTEGER and Type.NUMBER
		/// </summary>
		public double? Minimum { get; set; }
		/// <summary>
		/// Optional. Indicates if the value may be null.
		/// </summary>
		public bool? Nullable { get; set; }
		/// <summary>
		/// Optional. Pattern of the Type.STRING to restrict a string to a regular expression.
		/// </summary>
		public string? Pattern { get; set; }
		/// <summary>
		/// Optional. Properties of Type.OBJECT.
		/// </summary>
		public object? Properties { get; set; }
		/// <summary>
		/// Optional. The order of the properties. Not a standard field in open api spec. Used to determine the order of the properties in the response.
		/// </summary>
		public List<string>? PropertyOrdering { get; set; }
		/// <summary>
		/// Optional. Required properties of Type.OBJECT.
		/// </summary>
		public List<string>? Required { get; set; }
		/// <summary>
		/// Optional. The title of the schema.
		/// </summary>
		public string? Title { get; set; }
		/// <summary>
		/// Required. Data type.
		/// </summary>
		public Type? Type { get; set; }
    }
}