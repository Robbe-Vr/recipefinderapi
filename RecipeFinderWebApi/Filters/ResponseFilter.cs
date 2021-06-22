using Microsoft.AspNetCore.Mvc;
using RecipeFinderWebApi.UI.Models;
using RecipeFinderWebApi.Exchange.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Filters
{
    public static class ResponseFilter
    {
        public static IActionResult FilterDataResponse<T>(T data, Func<int, object, IActionResult> StatusCode)
        {
            ErrorData errData;
            if ((errData = NullReferenceCheck(data)).Error)
            {
                return StatusCode(errData.Code, errData.Data);
            }

            if ((errData = EmptyListCheck(data)).Error)
            {
                return StatusCode(errData.Code, errData.Data);
            }

            return StatusCode(200, data);
        }

        public static IActionResult FilterActionResponse<T>(T data, Func<int, object, IActionResult> StatusCode)
        {
            ErrorData errData;
            if ((errData = NullReferenceCheck(data)).Error)
            {
                return StatusCode(errData.Code, errData.Data);
            }

            if ((errData = ErrorCodeCheck(data)).Error)
            {
                return StatusCode(errData.Code, errData.Data);
            }

            return StatusCode(200, data);
        }

        private static ErrorData NullReferenceCheck<T>(T data)
        {
            if (data == null)
            {
                if ((typeof(T).IsGenericType && (typeof(T).GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                                                 typeof(T).GetGenericTypeDefinition() == typeof(List<>) ||
                                                 typeof(T).GetGenericTypeDefinition() == typeof(Array))) ||
                    typeof(T) == typeof(int) || typeof(T) == typeof(bool))
                {
                    return InternalError();
                }
                else if (typeof(T).Namespace.Contains("OrangeNXT"))
                {
                    return NoResults();
                }
            }
            else if (typeof(T) == typeof(string) && (String.IsNullOrEmpty(data as string) || String.IsNullOrWhiteSpace(data as string)))
            {
                return InternalError();
            }

            return new ErrorData();
        }

        private static ErrorData EmptyListCheck<T>(T data)
        {
            if (typeof(T).IsGenericType && (typeof(T).GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                                            typeof(T).GetGenericTypeDefinition() == typeof(List<>) ||
                                            typeof(T).GetGenericTypeDefinition() == typeof(Array)))
            {
                if ((data as IEnumerable).Cast<object>().Count() < 1)
                {
                    return NoResults();
                }
            }

            return new ErrorData();
        }

        private static ErrorData ErrorCodeCheck<T>(T data)
        {
            if (typeof(T) == typeof(int))
            {
                int code = int.Parse(data.ToString());

                if (code < 1)
                {
                    if (code == 0)
                    {
                        return ZeroChanges();
                    }
                    else if (code == -1)
                    {
                        return AlreadyExists();
                    }
                    else if (code == -2)
                    {
                        return EntityRestored();
                    }
                    else if (code == -3)
                    {
                        return UserNameAlreadyExists();
                    }
                    else if (code == -4)
                    {
                        return UserEmailAlreadyExists();
                    }
                }
            }

            return new ErrorData();
        }

        private static ErrorData NoResults()
        {
            ErrorModel model = new ErrorModel()
            {
                StatusCode = 404,
                Message = "Enumeration yielded no results. | Not Found.",
            };

            return new ErrorData(model.StatusCode, model);
        }

        private static ErrorData InternalError()
        {
            ErrorModel model = new ErrorModel()
            {
                StatusCode = 500,
                Message = "An internal server error occured.",
            };

            return new ErrorData(model.StatusCode, model);
        }

        private static ErrorData ZeroChanges()
        {
            ErrorModel model = new ErrorModel()
            {
                StatusCode = 200,
                Message = "The actions were performed, but zero changes were made to any of the resources.",
            };

            return new ErrorData(model.StatusCode, model);
        }

        private static ErrorData AlreadyExists()
        {
            ErrorModel model = new ErrorModel()
            {
                StatusCode = 200,
                Message = "An entity with the same property values already exists.",
            };

            return new ErrorData(model.StatusCode, model);
        }

        private static ErrorData EntityRestored()
        {
            ErrorModel model = new ErrorModel()
            {
                StatusCode = 200,
                Message = "A previously deleted entity with the same property values has been restored instead of creating or updating.",
            };

            return new ErrorData(model.StatusCode, model);
        }

        private static ErrorData UserNameAlreadyExists()
        {
            ErrorModel model = new ErrorModel()
            {
                StatusCode = 200,
                Message = "The given username is already in use.",
            };

            return new ErrorData(model.StatusCode, model);
        }

        private static ErrorData UserEmailAlreadyExists()
        {
            ErrorModel model = new ErrorModel()
            {
                StatusCode = 200,
                Message = "The given email address is already in use.",
            };

            return new ErrorData(model.StatusCode, model);
        }

        class ErrorData
        {
            public ErrorData(bool error = false)
            {
                Code = -1;
                Data = null;
                Error = error;
            }

            public ErrorData(int code, object data, bool error = true)
            {
                Code = code;
                Data = data;
                Error = error;
            }

            public bool Error { get; set; }
            public int Code { get; }
            public object Data { get; }
        }
    }
}
