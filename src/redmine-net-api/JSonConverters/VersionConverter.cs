/*
   Copyright 2011 - 2019 Adrian Popescu.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.Globalization;
#if !NET20
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Redmine.Net.Api.Extensions;
using Redmine.Net.Api.Types;
using Version = Redmine.Net.Api.Types.Version;

namespace Redmine.Net.Api.JSonConverters
{
    internal class VersionConverter : JavaScriptConverter
    {
        #region Overrides of JavaScriptConverter

        /// <summary>
        ///     When overridden in a derived class, converts the provided dictionary into an object of the specified type.
        /// </summary>
        /// <param name="dictionary">
        ///     An <see cref="System.Collections.Generic.IDictionary{TKey, TValue}" /> instance of property data stored
        ///     as name/value pairs.
        /// </param>
        /// <param name="type">The type of the resulting object.</param>
        /// <param name="serializer">The <see cref="System.Web.Script.Serialization.JavaScriptSerializer" /> instance.</param>
        /// <returns>
        ///     The deserialized object.
        /// </returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
            JavaScriptSerializer serializer)
        {
            if (dictionary != null)
            {
                var version = new Version();

                version.Id = dictionary.GetValue<int>(RedmineKeys.ID);
                version.Description = dictionary.GetValue<string>(RedmineKeys.DESCRIPTION);
                version.Name = dictionary.GetValue<string>(RedmineKeys.NAME);
                version.CreatedOn = dictionary.GetValue<DateTime?>(RedmineKeys.CREATED_ON);
                version.UpdatedOn = dictionary.GetValue<DateTime?>(RedmineKeys.UPDATED_ON);
                version.DueDate = dictionary.GetValue<DateTime?>(RedmineKeys.DUE_DATE);
                version.Project = dictionary.GetValueAsIdentifiableName(RedmineKeys.PROJECT);
                version.Sharing = dictionary.GetValue<VersionSharing>(RedmineKeys.SHARING);
                version.Status = dictionary.GetValue<VersionStatus>(RedmineKeys.STATUS);
                version.CustomFields = dictionary.GetValueAsCollection<IssueCustomField>(RedmineKeys.CUSTOM_FIELDS);

                return version;
            }

            return null;
        }

        /// <summary>
        ///     When overridden in a derived class, builds a dictionary of name/value pairs.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="serializer">The object that is responsible for the serialization.</param>
        /// <returns>
        ///     An object that contains key/value pairs that represent the object�s data.
        /// </returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var entity = obj as Version;

            var result = new Dictionary<string, object>();

            if (entity != null)
            {
                result.Add(RedmineKeys.NAME, entity.Name);
                result.Add(RedmineKeys.STATUS, entity.Status.ToString("G").ToLowerInv());
                result.Add(RedmineKeys.SHARING, entity.Sharing.ToString("G").ToLowerInv());
                result.Add(RedmineKeys.DESCRIPTION, entity.Description);

                var root = new Dictionary<string, object>();
                result.WriteDateOrEmpty(entity.DueDate, RedmineKeys.DUE_DATE);
                root[RedmineKeys.VERSION] = result;
                return root;
            }

            return result;
        }

        /// <summary>
        ///     When overridden in a derived class, gets a collection of the supported types.
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new[] {typeof(Version)}; }
        }

        #endregion
    }
}
#endif