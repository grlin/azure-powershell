// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Graph.RBAC.Version1_6.ActiveDirectory;
using Microsoft.Azure.Graph.RBAC.Models;
using Microsoft.WindowsAzure.Commands.Common;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using System;
using System.Management.Automation;
using System.Security;

namespace Microsoft.Azure.Commands.ActiveDirectory
{
    /// <summary>
    /// Updates an existing AD user.
    /// </summary>
    [Cmdlet(VerbsData.Update, ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ADUser", SupportsShouldProcess = true, DefaultParameterSetName = ParameterSet.UPNOrObjectId), OutputType(typeof(PSADUser))]
    [Alias("Set-" + ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ADUser")]
    public class UpdateAzureADUserCommand : ActiveDirectoryBaseCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "The userPrincipalName or ObjectId of the user to be updated.")]
        [ValidateNotNullOrEmpty]
        public string UPNOrObjectId { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "The user principal name of the user to be updated.")]
        [ValidateNotNullOrEmpty]
        [Alias("UPN")]
        public string UserPrincipalName { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "The object Id of the user to be updated.")]
        [ValidateNotNullOrEmpty]
        public Guid ObjectId { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = ParameterSet.InputObject, HelpMessage = "The object of the user to be updated.")]
        [ValidateNotNullOrEmpty]
        public PSADUser InputObject { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "This must be specified if you are using a federated domain for the user's userPrincipalName (UPN) property when creating a new user account.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "This must be specified if you are using a federated domain for the user's userPrincipalName (UPN) property when creating a new user account.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "This must be specified if you are using a federated domain for the user's userPrincipalName (UPN) property when creating a new user account.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "This must be specified if you are using a federated domain for the user's userPrincipalName (UPN) property when creating a new user account.")]
        [ValidateNotNullOrEmpty]
        public string ImmutableId { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "A two letter country code (ISO standard 3166). Required for users that will be assigned licenses.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "A two letter country code (ISO standard 3166). Required for users that will be assigned licenses.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "A two letter country code (ISO standard 3166). Required for users that will be assigned licenses.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "A two letter country code (ISO standard 3166). Required for users that will be assigned licenses.")]
        [ValidateNotNullOrEmpty]
        public string UsageLocation { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "The user's given name.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "The user's given name.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "The user's given name.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "The user's given name.")]
        [ValidateNotNullOrEmpty]
        public string GivenName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "The user's surname.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "The user's surname.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "The user's surname.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "The user's surname.")]
        [ValidateNotNullOrEmpty]
        public string Surname { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "A string value that can be used to classify user types in your directory, such as 'Member' or 'Guest'.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "A string value that can be used to classify user types in your directory, such as 'Member' or 'Guest'.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "A string value that can be used to classify user types in your directory, such as 'Member' or 'Guest'.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "A string value that can be used to classify user types in your directory, such as 'Member' or 'Guest'.")]
        [ValidateNotNullOrEmpty]
        public string UserType { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "New display name for the user.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "New display name for the user.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "New display name for the user.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "New display name for the user.")]
        [ValidateNotNullOrEmpty]
        public string DisplayName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "true for enabling the account; otherwise, false.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "true for enabling the account; otherwise, false.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "true for enabling the account; otherwise, false.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "true for enabling the account; otherwise, false.")]
        [ValidateNotNullOrEmpty]
        public bool? EnableAccount { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "The mail alias for the user.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "The mail alias for the user.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "The mail alias for the user.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "The mail alias for the user.")]
        [ValidateNotNullOrEmpty]
        public string MailNickname { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "New password for the user.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.UPN, HelpMessage = "New password for the user.")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "New password for the user.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "New password for the user.")]
        [ValidateNotNullOrEmpty]
        public SecureString Password { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.UPNOrObjectId, HelpMessage = "It must be specified if the user should change the password on the next successful login. Only valid if password is updated otherwise it will be ignored.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.UPN, HelpMessage = "It must be specified if the user should change the password on the next successful login. Only valid if password is updated otherwise it will be ignored.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.ObjectId, HelpMessage = "It must be specified if the user should change the password on the next successful login. Only valid if password is updated otherwise it will be ignored.")]
        [Parameter(Mandatory = false, ParameterSetName = ParameterSet.InputObject, HelpMessage = "It must be specified if the user should change the password on the next successful login. Only valid if password is updated otherwise it will be ignored.")]
        public SwitchParameter ForceChangePasswordNextLogin { get; set; }

        public override void ExecuteCmdlet()
        {
            PasswordProfile profile = null;
            if (Password != null && Password.Length > 0)
            {
                string decodedPassword = SecureStringExtensions.ConvertToString(Password);
                profile = new PasswordProfile
                {
                    Password = decodedPassword,
                    ForceChangePasswordNextLogin = ForceChangePasswordNextLogin.IsPresent ? true : false
                };
            }

            var userUpdateParameters = new UserUpdateParameters(null, null, null, null, null, null, EnableAccount, DisplayName, profile, null);

            if (this.IsParameterBound(c => c.ImmutableId))
            {
                userUpdateParameters.ImmutableId = ImmutableId;
            }

            if (this.IsParameterBound(c => c.UsageLocation))
            {
                userUpdateParameters.UsageLocation = UsageLocation;
            }

            if (this.IsParameterBound(c => c.GivenName))
            {
                userUpdateParameters.GivenName = GivenName;
            }

            if (this.IsParameterBound(c => c.Surname))
            {
                userUpdateParameters.Surname = Surname;
            }

            if (this.IsParameterBound(c => c.UserType))
            {
                userUpdateParameters.UserType = UserType;
            }

            if (this.IsParameterBound(c => c.MailNickname))
            {
                userUpdateParameters.MailNickname = MailNickname;
            }

            ExecutionBlock(() =>
            {
                if (this.IsParameterBound(c => c.InputObject))
                {
                    UPNOrObjectId = !string.IsNullOrEmpty(InputObject.UserPrincipalName) ?
                                        InputObject.UserPrincipalName :
                                        InputObject.Id.ToString();
                }
                else if (this.IsParameterBound(c => c.UserPrincipalName))
                {
                    UPNOrObjectId = UserPrincipalName;
                }
                else if (this.IsParameterBound(c => c.ObjectId))
                {
                    UPNOrObjectId = ObjectId.ToString();
                }

                if (ShouldProcess(target: UPNOrObjectId, action: string.Format("Updating properties for user with upn or object id '{0}'", UPNOrObjectId)))
                {
                    WriteObject(ActiveDirectoryClient.UpdateUser(UPNOrObjectId, userUpdateParameters));
                }
            });
        }
    }
}
