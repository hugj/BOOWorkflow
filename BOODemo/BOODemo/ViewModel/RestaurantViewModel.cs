﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.sysu.workflow;
using com.sysu.workflow.io;
using com.sysu.workflow.env;
using com.sysu.workflow.model;
using com.sysu.workflow.env.jexl;
using BOODemo.Core;

namespace BOODemo.ViewModel
{
    extern alias OpenJDKCore;
    /// <summary>
    /// ViewModel类
    /// </summary>
    internal static class RestaurantViewModel
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static RestaurantViewModel()
        {
            RestaurantViewModel.engineBridge = EngineBridge.GetInstance();
            RestaurantViewModel.messageHandler = new StateMachineMessageHandler();
            RestaurantViewModel.engineBridge.Init(messageHandler);
            RestaurantViewModel.executorDict = new Dictionary<int, SCXMLExecutor>();
        }

        /// <summary>
        /// 开始一个新的业务对象实例
        /// </summary>
        public static void CreateNewBussinessObjectInstance()
        {
            StateMachineMessageHandler MsgHandler = new StateMachineMessageHandler();
            OpenJDKCore.java.net.URL url = new OpenJDKCore.java.net.URL("file", "", "GuestOrder.xml");
            SCXML scxml = SCXMLReader.read(url);
            Evaluator ev = new JexlEvaluator();
            SCXMLExecutor executor = new SCXMLExecutor(ev, new MulitStateMachineDispatcher(), new SimpleErrorReporter());
            executor.setStateMachine(scxml);
            RestaurantViewModel.engineBridge.SetExecutorReference(RestaurantViewModel.executorCounter, executor);
            RestaurantViewModel.executorDict[RestaurantViewModel.executorCounter++] = executor;
        }

        /// <summary>
        /// 状态机执行器字典
        /// </summary>
        private static Dictionary<int, SCXMLExecutor> executorDict;

        /// <summary>
        /// 状态机执行器计数
        /// </summary>
        private static int executorCounter = 0;

        /// <summary>
        /// 桥接器
        /// </summary>
        private static EngineBridge engineBridge;

        /// <summary>
        /// 消息处理器
        /// </summary>
        private static StateMachineMessageHandler messageHandler;
    }
}