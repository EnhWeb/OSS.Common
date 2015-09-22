﻿using System;
using System.Text;
using OS.Common.ComModels.Enums;
using OS.Common.ComUtils;
using OS.Common.Encrypt;
using OS.Common.Extention;

namespace OS.Common.Authrization
{

    /// <summary>
    ///   授权认证信息
    /// </summary>
    public class SysAuthorizeInfo
    {
        #region  参与签名属性

        /// <summary>
        ///   应用版本
        ///    不得包含  & 等特殊符号
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        ///   应用来源
        /// </summary>
        public string AppSource { get; set; }

        /// <summary>
        /// 应用客户端类型
        /// </summary>
        public AppClient AppClient { get; set; }

        /// <summary>
        /// 设备ID
        ///  不得包含  & 等特殊符号
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        ///  Token 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public long TimeSpan { get; set; }

        /// <summary>
        ///  sign标识
        /// </summary>
        public string Sign { get; set; }


        /// <summary>
        /// IP地址   可选
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 浏览器类型   可选
        /// </summary>
        public WebBrowserClient WebBrowser { get; set; }


        /// <summary>
        /// 原始appsource   可选
        /// 主要是当应用层向基础层传递时使用
        ///  如支付系统等，api层面对多个应用，每个应用对应不同支付key，调用支付接口时必传
        /// </summary>
        public string OriginAppSource { get; set; }

        #endregion

        /// <summary>
        ///  是否是新版
        /// </summary>
        public bool IsNew { get; set; }
        // 是否是新版加密方式，默认是true
        private static string newFlag = "masterauth";
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysAuthorizeInfo()
        {
            IsNew = true;
        }



        #region  字符串处理

        /// <summary>
        ///   从头字符串中初始化签名相关属性信息
        /// </summary>
        /// <param name="signData"></param>
        /// <param name="separator">A=a  B=b 之间分隔符</param>
        public void FromSignData(string signData, char separator=';')
        {
            if (!string.IsNullOrEmpty(signData))
            {
                IsNew = signData.StartsWith(newFlag + separator);

                string[] strs = signData.Split(separator);
                foreach (var str in strs)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        string[] keyValue = str.Split(new[] { '=' }, 2);
                        if (keyValue.Length > 1)
                        {
                            string val = keyValue[1].UrlDecode();
                            switch (keyValue[0].ToLower())
                            {
                                case "appversion":
                                    AppVersion = val;
                                    break;
                                case "token":
                                    Token = val;
                                    break;
                                case "appsource":
                                    AppSource = val;
                                    break;
                                case "appclient":
                                    AppClient = (AppClient)val.ToInt32();
                                    break;
                                case "sign":
                                    Sign = val;
                                    break;
                                case "deviceid":
                                    DeviceId = val;
                                    break;
                                case "timespan":
                                    TimeSpan = val.ToInt64();
                                    break;
                                case "ipaddress":
                                    IpAddress = val;
                                    break;
                                case "webbrowser":
                                    WebBrowser = (WebBrowserClient)val.ToInt32();
                                    break;
                                case "originappsource":
                                    OriginAppSource = val;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成签名后的字符串
        /// </summary>
        /// <returns></returns>
        public string ToSignData(string secretKey, char separator=';')
        {
            IsNew = true; //  如果客户端是旧加密方式，生成串时要处理成新版

            TimeSpan = DateTime.Now.ToUtcSeconds();

            var encrpStr = GetSignContent(separator);
            Sign = HmacSha1.EncryptBase64(encrpStr.ToString(), secretKey);

            AddSignDataValue("sign", Sign, separator, encrpStr);

            return encrpStr.ToString();
        }

        /// <summary>
        /// 复制新的授权信息实体
        /// </summary>
        /// <returns></returns>
        public SysAuthorizeInfo Copy()
        {
            SysAuthorizeInfo newOne = new SysAuthorizeInfo();

            newOne.AppClient = this.AppClient;
            newOne.AppSource = this.AppSource;
            newOne.AppVersion = this.AppVersion;
            newOne.DeviceId = this.DeviceId;
            newOne.IpAddress = this.IpAddress;

            newOne.OriginAppSource = this.OriginAppSource;
            newOne.Sign = this.Sign;
            newOne.TimeSpan = this.TimeSpan;
            newOne.Token = this.Token;
            newOne.WebBrowser = this.WebBrowser;
            return newOne;
        }

        #endregion

        #region  签名相关

        /// <summary>
        ///   检验是否合法
        /// </summary>
        /// <returns></returns>
        public bool CheckSign(string secretKey, char separator=';')
        {
            var strTicketParas = GetSignContent(separator);

            //  如果是新版 已经 url编码处理，不需要特殊处理
            string signData = IsNew ? 
                HmacSha1.EncryptBase64(strTicketParas.ToString(), secretKey) 
                : HmacSha1.EncryptBase64(strTicketParas.ToString(), secretKey).Base64UrlEncode();

            return Sign == signData;
        }

        /// <summary>
        ///   获取要加密签名的串
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        private StringBuilder GetSignContent(char separator)
        {
            StringBuilder strTicketParas = new StringBuilder();

            if (IsNew)
            { 
                //  照顾旧客户端加密方式，否则检验时串不一样
                strTicketParas.Append(newFlag);
            }

            AddSignDataValue("appclient", ((int)AppClient).ToString(), separator, strTicketParas);
            AddSignDataValue("appsource", AppSource.ToString(), separator, strTicketParas);
            AddSignDataValue("appversion", AppVersion, separator, strTicketParas);
            AddSignDataValue("deviceid", DeviceId, separator, strTicketParas);

            AddSignDataValue("ipaddress", IpAddress, separator, strTicketParas);
            AddSignDataValue("timespan", TimeSpan.ToString(), separator, strTicketParas);
            AddSignDataValue("token", Token, separator, strTicketParas);
            if (WebBrowser != WebBrowserClient.None)
            {
                AddSignDataValue("webbrowser", ((int)WebBrowser).ToString(), separator, strTicketParas);
            }
       
               AddSignDataValue("originappsource", OriginAppSource, separator, strTicketParas);
          
            return strTicketParas;
        }

        /// <summary>
        ///   追加要加密的串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <param name="strTicketParas"></param>
        private void AddSignDataValue(string name, string value, char separator, StringBuilder strTicketParas)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (strTicketParas.Length > 0)
                {
                    strTicketParas.Append(separator);
                }
                strTicketParas.Append(name).Append("=").Append(IsNew?value.UrlEncode():value);
            }
        }

        #endregion

    }

}