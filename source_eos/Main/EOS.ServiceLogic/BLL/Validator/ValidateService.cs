using System;
using System.Collections.Generic;
using System.Linq;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;
using Newtonsoft.Json;

namespace EOS.ServiceLogic.BLL.Validator
{
    public class ValidateService<T>
    {
        //Nombres de las propiedades DateTime en string
        public string DateFromProp { get; set; }
        public string DateToProp { get; set; }

        //Nombre de las propiedades DateTime en DateTime
        public string DateTimeFromProp { get; set; }
        public string DateTimeToProp { get; set; }

        //Nombres de las propiedades DateTime en string
        public string DateFromProp2 { get; set; }
        public string DateToProp2 { get; set; }

        //Nombre de las propiedades DateTime en DateTime
        public string DateTimeFromProp2 { get; set; }
        public string DateTimeToProp2 { get; set; }


        public T Validate(QSuscriptor suscriptor, string data, ref ServiceError serviceError, string token)
        {

            //Compruebo que al desencriptar no devuelva null
            var jsonData = Utility.Decrypt3DES(suscriptor, data, token);
            if (string.IsNullOrEmpty(jsonData))
            {
                serviceError.IsError = true;
                serviceError.ErrorList = new List<ServiceErrorItem>()
                {
                    new ServiceErrorItem()
                    {
                        Code = ErrorCode.NullFilter.GetHashCode(),
                        Description = Utility.GetDisplayName(typeof(ErrorCode),
                            ErrorCode.NullFilter.ToString())
                    }
                };

                return default(T);
            }
            var filter = JsonConvert.DeserializeObject<T>(jsonData);
            //Si el filtro es null devuelvo false
            if (filter == null)
            {
                serviceError.IsError = true;
                serviceError.ErrorList =
                    new List<ServiceErrorItem>()
                    {
                        new ServiceErrorItem()
                        {
                            Code = ErrorCode.NullFilter.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.NullFilter.ToString())
                        }
                    };
                return default(T);
            }


            //Transformo las fechas de texto a DateTime para comprobar si son válidas y para almacenarlas en el objeto del filtro
            var dateFrom = DateTime.MinValue;
            var dateTo = DateTime.MinValue;
            try
            {
                if (!string.IsNullOrEmpty(DateFromProp) && !string.IsNullOrEmpty(DateTimeFromProp))
                {
                    var dateStringFromInfo = filter.GetType().GetProperty(DateFromProp);
                    if (dateStringFromInfo != null)
                    {
                        var dateFromValue = (string)dateStringFromInfo.GetValue(filter, null);
                        if (!string.IsNullOrEmpty(dateFromValue))
                        {
                            dateFrom = new DateTime(int.Parse(dateFromValue.Substring(0, 4)),
                                int.Parse(dateFromValue.Substring(4, 2)),
                                int.Parse(dateFromValue.Substring(6, 2)));
                        }
                        else
                        {
                            dateFrom = DateTime.MinValue;
                        }
                        filter.GetType().GetProperty(DateTimeFromProp).SetValue(filter, dateFrom, null);

                    }
                }

                if (!string.IsNullOrEmpty(DateToProp) && !string.IsNullOrEmpty(DateTimeToProp))
                {
                    var dateStringToInfo = filter.GetType().GetProperty(DateToProp);
                    if (dateStringToInfo != null)
                    {
                        var dateToValue = (string)dateStringToInfo.GetValue(filter, null);
                        if (!string.IsNullOrEmpty(dateToValue))
                        {
                            dateTo = new DateTime(int.Parse(dateToValue.Substring(0, 4)),
                                int.Parse(dateToValue.Substring(4, 2)),
                                int.Parse(dateToValue.Substring(6, 2)));
                        }
                        else
                        {
                            dateTo = DateTime.MinValue;
                        }
                        filter.GetType().GetProperty(DateTimeToProp).SetValue(filter, dateTo, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Excepción en ValidateService: " + ex.Message);
                Quodem.Monitor.Alerta.SendNotificacion("Excepción en ValidateService: " + ex.Message);
                serviceError.ErrorList.Add(new ServiceErrorItem()
                {
                    Code = ErrorCode.DateFormatError.GetHashCode(),
                    Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DateFormatError.ToString())
                });
            }

            //Compruebo si son un rango correcto de fechas
            if (dateTo != DateTime.MinValue && dateFrom != DateTime.MinValue && dateTo < dateFrom)
            {
                serviceError.ErrorList.Add(
                    new ServiceErrorItem()
                    {
                        Code = ErrorCode.DateFromBiggerThanDateTo.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.DateFromBiggerThanDateTo.ToString())
                    });
            }

            //---------------------------------
            var dateFrom2 = DateTime.MinValue;
            var dateTo2 = DateTime.MinValue;
            try
            {
                if (!string.IsNullOrEmpty(DateFromProp2) && !string.IsNullOrEmpty(DateTimeFromProp2))
                {
                    var dateStringFromInfo = filter.GetType().GetProperty(DateFromProp2);
                    if (dateStringFromInfo != null)
                    {
                        var dateFromValue = (string)dateStringFromInfo.GetValue(filter, null);
                        if (!string.IsNullOrEmpty(dateFromValue))
                        {
                            dateFrom2 = DateTime.ParseExact(dateFromValue, Variables.DATE_TIME_FORMAT, null);
                        }
                        else
                        {
                            dateFrom2 = DateTime.MinValue;
                        }
                        filter.GetType().GetProperty(DateTimeFromProp2).SetValue(filter, dateFrom2, null);

                    }
                }

                if (!string.IsNullOrEmpty(DateToProp2) && !string.IsNullOrEmpty(DateTimeToProp2))
                {
                    var dateStringToInfo = filter.GetType().GetProperty(DateToProp2);
                    if (dateStringToInfo != null)
                    {
                        var dateToValue = (string)dateStringToInfo.GetValue(filter, null);
                        if (!string.IsNullOrEmpty(dateToValue))
                        {
                            dateTo2 = DateTime.ParseExact(dateToValue, Variables.DATE_TIME_FORMAT, null);
                        }
                        else
                        {
                            dateTo2 = DateTime.MinValue;
                        }
                        filter.GetType().GetProperty(DateTimeToProp2).SetValue(filter, dateTo2, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Excepción en ValidateService: " + ex.Message);
                Quodem.Monitor.Alerta.SendNotificacion("Excepción en ValidateService: " + ex.Message);
                serviceError.ErrorList.Add(new ServiceErrorItem()
                {
                    Code = ErrorCode.DateFormatError.GetHashCode(),
                    Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DateFormatError.ToString())
                });
            }

            //Compruebo si son un rango correcto de fechas
            if (dateTo2 != DateTime.MinValue && dateFrom2 != DateTime.MinValue && dateTo2 < dateFrom2)
            {
                serviceError.ErrorList.Add(
                    new ServiceErrorItem()
                    {
                        Code = ErrorCode.DateFromBiggerThanDateTo.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.DateFromBiggerThanDateTo.ToString())
                    });
            }
            //---------------------------------

            //Compruebo las condiciones establecidas mediante DataAnnotations
            var errorList = ObjectValidator<T>.ValidateObject(filter);
            if (!serviceError.IsError)
            {
                serviceError.IsError = errorList.Any();
            }

            foreach (var errorDescription in errorList)
            {
                serviceError.ErrorList.Add(new ServiceErrorItem()
                {
                    Code = ErrorCode.DataNotValid.GetHashCode(),
                    Description = errorDescription
                });
            }

            return filter;
        }
    }
}
