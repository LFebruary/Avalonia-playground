﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Playground.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SerialPort {
            get {
                return ((string)(this["SerialPort"]));
            }
            set {
                this["SerialPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9600")]
        public int BaudRate {
            get {
                return ((int)(this["BaudRate"]));
            }
            set {
                this["BaudRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int Databits {
            get {
                return ((int)(this["Databits"]));
            }
            set {
                this["Databits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int Stop_bits {
            get {
                return ((int)(this["Stop_bits"]));
            }
            set {
                this["Stop_bits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public string Parity {
            get {
                return ((string)(this["Parity"]));
            }
            set {
                this["Parity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Flow_control {
            get {
                return ((string)(this["Flow_control"]));
            }
            set {
                this["Flow_control"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Stability_indicator_active {
            get {
                return ((bool)(this["Stability_indicator_active"]));
            }
            set {
                this["Stability_indicator_active"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Sequence_of_identical_readings_active {
            get {
                return ((bool)(this["Sequence_of_identical_readings_active"]));
            }
            set {
                this["Sequence_of_identical_readings_active"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ST")]
        public string Stability_indicator_snippet {
            get {
                return ((string)(this["Stability_indicator_snippet"]));
            }
            set {
                this["Stability_indicator_snippet"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-1")]
        public int Stability_indicator_starting_position {
            get {
                return ((int)(this["Stability_indicator_starting_position"]));
            }
            set {
                this["Stability_indicator_starting_position"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int Number_of_identical_readings {
            get {
                return ((int)(this["Number_of_identical_readings"]));
            }
            set {
                this["Number_of_identical_readings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("11")]
        public int Scale_string_weight_start_position {
            get {
                return ((int)(this["Scale_string_weight_start_position"]));
            }
            set {
                this["Scale_string_weight_start_position"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("18")]
        public int Scale_string_weight_end_position {
            get {
                return ((int)(this["Scale_string_weight_end_position"]));
            }
            set {
                this["Scale_string_weight_end_position"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("17")]
        public int Scale_string_minimum_length {
            get {
                return ((int)(this["Scale_string_minimum_length"]));
            }
            set {
                this["Scale_string_minimum_length"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection Received_values {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["Received_values"]));
            }
            set {
                this["Received_values"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Scale_string_must_conform_to_length {
            get {
                return ((bool)(this["Scale_string_must_conform_to_length"]));
            }
            set {
                this["Scale_string_must_conform_to_length"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Take_full_scale_string {
            get {
                return ((bool)(this["Take_full_scale_string"]));
            }
            set {
                this["Take_full_scale_string"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5050")]
        public int BroadcastPort {
            get {
                return ((int)(this["BroadcastPort"]));
            }
            set {
                this["BroadcastPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public int Serial_timeout_ms {
            get {
                return ((int)(this["Serial_timeout_ms"]));
            }
            set {
                this["Serial_timeout_ms"] = value;
            }
        }
    }
}
