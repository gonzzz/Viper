﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Viper.Framework.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class SyntaxErrorMessagesEN {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SyntaxErrorMessagesEN() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Viper.Framework.Resources.SyntaxErrorMessagesEN", typeof(SyntaxErrorMessagesEN).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Transaction next time cannot be lower than current System Time.
        /// </summary>
        public static string EXCEPTION_ADVANCE_TRANSACTION_NEXT_TIME {
            get {
                return ResourceManager.GetString("EXCEPTION_ADVANCE_TRANSACTION_NEXT_TIME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Transaction arrival time cannot be lower than current System Time.
        /// </summary>
        public static string EXCEPTION_GENERATE_TRANSACTION_ARRIVAL_TIME {
            get {
                return ResourceManager.GetString("EXCEPTION_GENERATE_TRANSACTION_ARRIVAL_TIME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An exception has occured parsing a block in line {0}. Message: {1}..
        /// </summary>
        public static string EXCEPTION_ON_BLOCK_PARSING_NO_BLOCK_NAME {
            get {
                return ResourceManager.GetString("EXCEPTION_ON_BLOCK_PARSING_NO_BLOCK_NAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An exception has occurred parsing block &apos;{0}&apos; in line {1}. Message: {2}..
        /// </summary>
        public static string EXCEPTION_ON_BLOCK_PARSING_WITH_BLOCK_NAME {
            get {
                return ResourceManager.GetString("EXCEPTION_ON_BLOCK_PARSING_WITH_BLOCK_NAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The block in line {0} is invalid or not supported by Viper. Please proceed to fix it to continue with the creation of the simulation model. .
        /// </summary>
        public static string INVALID_OR_NOT_SUPPORTED_BLOCK {
            get {
                return ResourceManager.GetString("INVALID_OR_NOT_SUPPORTED_BLOCK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The block &apos;{0}&apos; in line {1} has a wrong syntax. Please proceed to fix it to continue with the creation of the simulation model..
        /// </summary>
        public static string WRONG_BLOCK_SYNTAX {
            get {
                return ResourceManager.GetString("WRONG_BLOCK_SYNTAX", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The SNA &apos;{0}&apos; used in line {1} is not allowed. Pleas proceed to fix it to continue with the execution fo the simulation model..
        /// </summary>
        public static string WRONG_SNA_UTILIZATION {
            get {
                return ResourceManager.GetString("WRONG_SNA_UTILIZATION", resourceCulture);
            }
        }
    }
}
