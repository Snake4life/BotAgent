﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BotAgent.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IServiceClass")]
    public interface IServiceClass {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceClass/saveStatistic", ReplyAction="http://tempuri.org/IServiceClass/saveStatisticResponse")]
        void saveStatistic(string botName, string rows);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceClass/saveStatistic", ReplyAction="http://tempuri.org/IServiceClass/saveStatisticResponse")]
        System.Threading.Tasks.Task saveStatisticAsync(string botName, string rows);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceClass/getCurrentStatByBotName", ReplyAction="http://tempuri.org/IServiceClass/getCurrentStatByBotNameResponse")]
        string getCurrentStatByBotName(string botName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceClass/getCurrentStatByBotName", ReplyAction="http://tempuri.org/IServiceClass/getCurrentStatByBotNameResponse")]
        System.Threading.Tasks.Task<string> getCurrentStatByBotNameAsync(string botName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceClassChannel : BotAgent.ServiceReference1.IServiceClass, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClassClient : System.ServiceModel.ClientBase<BotAgent.ServiceReference1.IServiceClass>, BotAgent.ServiceReference1.IServiceClass {
        
        public ServiceClassClient() {
        }
        
        public ServiceClassClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClassClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClassClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClassClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void saveStatistic(string botName, string rows) {
            base.Channel.saveStatistic(botName, rows);
        }
        
        public System.Threading.Tasks.Task saveStatisticAsync(string botName, string rows) {
            return base.Channel.saveStatisticAsync(botName, rows);
        }
        
        public string getCurrentStatByBotName(string botName) {
            return base.Channel.getCurrentStatByBotName(botName);
        }
        
        public System.Threading.Tasks.Task<string> getCurrentStatByBotNameAsync(string botName) {
            return base.Channel.getCurrentStatByBotNameAsync(botName);
        }
    }
}
