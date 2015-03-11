﻿

using OS.Common.ComModels.Enums;

namespace OS.Common.ComModels
{
    public class ResultModel
    {
        /// <summary>
        /// 空构造函数
        /// </summary>
        public ResultModel()
        {
            Ret = ResultTypes.Success;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="message"></param>
        public ResultModel(int ret, string message = "")
        {
            Ret = (ResultTypes)ret;
            Message = message;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="message"></param>
        public ResultModel(ResultTypes ret, string message = "")
        {
            this.Ret = ret;
            this.Message = message;
        }


        /// <summary>
        /// 返回结果
        ///  2xx   成功相关状态（如： 200）
        ///  3xx   参数相关错误 
        ///  4xx   用户授权相关错误
        ///  5xx   服务器内部相关错误信息
        ///  6xx   系统级定制错误信息，如升级维护等
        /// </summary>
        public ResultTypes Ret { get; set; }

        /// <summary>
        /// 错误或者状态
        /// </summary>
        public string Message { get; set; }
    }


    public class ResultIdModel : ResultModel
    {
        public ResultIdModel()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        public ResultIdModel(long id)
            : base(200, null)
        {
            Id = id;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="message"></param>
        public ResultIdModel(int ret, string message = ""):base(ret,message)
        {
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="message"></param>
        public ResultIdModel(ResultTypes ret, string message = "")
            : base(ret, message)
        {
        }

        /// <summary>
        /// 返回的关键值，如返回添加是否成功并返回id
        /// </summary>
        public long Id { get; set; }
    }



    public class ResultModel<TType> : ResultModel
    {
        public ResultModel()
        {
        
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public ResultModel(TType data)
            : base(200, null)
        {
            Data = data;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="message"></param>
        public ResultModel(int ret, string message = "")
            : base(ret, message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="message"></param>
        public ResultModel(ResultTypes ret, string message = "")
            : base(ret, message)
        {
        }

        /// <summary>
        /// 返回指定类型的数据
        /// </summary>
        public TType Data { get; set; }
    }


    public static class ResultModelMap
    {
        /// <summary>
        ///   将结果实体基本属性（ret，message）赋值
        /// </summary>
        /// <typeparam name="TResult">输出对象</typeparam>
        /// <returns>输出对象</returns>
        public static TResult ConvertBaseTo<TResult>(this ResultModel source)
            where TResult : ResultModel, new()
        {
            TResult ot = new TResult();
            ot.Ret = source.Ret;
            ot.Message = source.Message;
            return ot;
        }
    }
}
