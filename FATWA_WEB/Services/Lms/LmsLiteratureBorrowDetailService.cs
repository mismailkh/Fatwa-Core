using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_WEB.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services.Lms
{
	//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master"> Lms Literature BorrowDetail Service for communicating with API Literature Borrow Details Controller</History>
	public partial class LmsLiteratureBorrowDetailService
	{
		private readonly IConfiguration _config;
		private readonly ILocalStorageService _browserStorage;

		private readonly NavigationManager navigationManager;
		private readonly TranslationState translationState;

		public LmsLiteratureBorrowDetailService(IConfiguration configuration, NavigationManager _navigationManager, ILocalStorageService browserStorage, TranslationState _translationState)
		{
			_config = configuration;
			navigationManager = _navigationManager;
			_browserStorage = browserStorage;
			translationState = _translationState;
		}
		partial void OnLmsLiteratureDetailsRead(ref IQueryable<LmsLiterature> items);


		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master"> Get List of Literature Borrow Details</History>
		public async Task<ApiCallResponse> GetLmsLiteratureBorrowDetails(Query query = null)
		{
			var response = await GetLmsLiteratureBorrowDetails();
			if (response.IsSuccessStatusCode)
			{
                var data = (IEnumerable<BorrowDetailVM>)response.ResultData;
                var items = data.AsQueryable();
				if (query != null)
				{
					if (!string.IsNullOrEmpty(query.Expand))
					{
						var propertiesToExpand = query.Expand.Split(',');
						foreach (var p in propertiesToExpand)
						{
							items = items.Include(p);
						}
					}

					if (!string.IsNullOrEmpty(query.Filter))
					{
						if (query.FilterParameters != null)
						{
							items = items.Where(query.Filter, query.FilterParameters);
						}
						else
						{
							items = items.Where(query.Filter);
						}
					}

					if (!string.IsNullOrEmpty(query.OrderBy))
					{
						items = items.OrderBy(query.OrderBy);
					}

					if (query.Skip.HasValue)
					{
						items = items.Skip(query.Skip.Value);
					}

					if (query.Top.HasValue)
					{
						items = items.Take(query.Top.Value);
					}
				}

				OnLmsLiteratureBorrowDetailsRead(ref items);

				return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = items };
			}
			else
			{
				return response;
			}
		}

		//<History Author = 'Nabeel ur Rehman' Date='2023-02-23' Version="1.0" Branch="master"> Get List of Literature Return Details</History>
		public async Task<ApiCallResponse> GetLmsLiteratureReturnDetails(Query query = null)
		{
			var response = await GetLmsLiteratureReturnDetails();
			if (response.IsSuccessStatusCode)
			{
				var data = (IEnumerable<ReturnDetailVM>)response.ResultData;
                var items = data.AsQueryable();
                if (query != null)
				{
					if (!string.IsNullOrEmpty(query.Expand))
					{
						var propertiesToExpand = query.Expand.Split(',');
						foreach (var p in propertiesToExpand)
						{
							items = items.Include(p);
						}
					}

					if (!string.IsNullOrEmpty(query.Filter))
					{
						if (query.FilterParameters != null)
						{
							items = items.Where(query.Filter, query.FilterParameters);
						}
						else
						{
							items = items.Where(query.Filter);
						}
					}

					if (!string.IsNullOrEmpty(query.OrderBy))
					{
						items = items.OrderBy(query.OrderBy);
					}

					if (query.Skip.HasValue)
					{
						items = items.Skip(query.Skip.Value);
					}

					if (query.Top.HasValue)
					{
						items = items.Take(query.Top.Value);
					}
				}
				OnLmsLiteratureBorrowDetailsRead(ref items);
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = items };
            }
			else
			{
				return response;
			}
		}
		partial void OnLmsLiteratureBorrowDetailsRead(ref IQueryable<ReturnDetailVM> items);
		partial void OnLmsLiteratureBorrowDetailsRead(ref IQueryable<BorrowDetailVM> items);

		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master"> Call API for getting List of Literature Borrow Details</History>
		public async Task<ApiCallResponse> GetLmsLiteratureBorrowDetails()
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetLmsLiteratureBorrowDetails");
				var postBody = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
				request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				var response = await new HttpClient().SendAsync(request);
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					throw new Exception(translationState.Translate("Contact_Administrator"));
				}
                else if (response.IsSuccessStatusCode)
                {
                    var responselist = response.Content.ReadFromJsonAsync<IEnumerable<BorrowDetailVM>>();
                    var queryableX = (await responselist).AsQueryable();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = queryableX };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		//<History Author = 'Nabeel ur Rehman' Date='2022-03-22' Version="1.0" Branch="master"> Call API for getting List of Literature Return Details</History>
		public async Task<ApiCallResponse> GetLmsLiteratureReturnDetails()
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetLmsLiteratureReturnDetails");
				var postBody = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
				request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				var response = await new HttpClient().SendAsync(request);
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					throw new Exception(translationState.Translate("Contact_Administrator"));
				}
                else if (response.IsSuccessStatusCode)
				{
                    var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ReturnDetailVM>>();
                    var queryableX = (await responselist).AsQueryable();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = queryableX };
                }
				else
				{
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}

		private async Task<IQueryable<LmsLiterature>> GetLmsLiteratureDetails()
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsLiteratures");
				// add authorization header
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				// send request
				var response = await new HttpClient().SendAsync(request);
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					throw new Exception(translationState.Translate("Contact_Administrator"));
				}
				var responselist = response.Content.ReadFromJsonAsync<IEnumerable<LmsLiterature>>();
				var queryableX = (await responselist).AsQueryable();
				return queryableX;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<ApiCallResponse> GetLmsLiteratureBySearchTerm(string filter, string appCulture)
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsLiteraturesBySearchTerm?searchTerm=" + filter + "&appCulture=" + appCulture);
				// add authorization header
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				// send request
				var response = await new HttpClient().SendAsync(request);
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					throw new Exception(translationState.Translate("Contact_Administrator"));
				}
				else if (response.IsSuccessStatusCode)
				{
                    var responselist = await response.Content.ReadFromJsonAsync<IEnumerable<LiteratureDetailVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = responselist };
                }
				else
				{
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}

		#region Literature Borrow

		#region Create

		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master"> Creating Literature Borrow Details</History>
		public async Task<ApiCallResponse> CreateLmsLiteratureBorrowDetail(LmsLiteratureBorrowDetail lmsLiteratureBorrowDetail)
		{
			OnLmsLiteratureBorrowDetailCreated(lmsLiteratureBorrowDetail);
			try
			{
				var response = await SubmitLmsLiteratureBorrowDetail(lmsLiteratureBorrowDetail);
				if (response.IsSuccessStatusCode)
				{
					OnAfterLmsLiteratureBorrowDetailCreated(lmsLiteratureBorrowDetail);
					return response;
				}
				else
				{
					return response;
				}
			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}
		partial void OnLmsLiteratureBorrowDetailCreated(LmsLiteratureBorrowDetail item);
		partial void OnAfterLmsLiteratureBorrowDetailCreated(LmsLiteratureBorrowDetail item);

		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master"> Call API for creating Literature Borrow Details</History>
		protected async Task<ApiCallResponse> SubmitLmsLiteratureBorrowDetail(LmsLiteratureBorrowDetail borrowDetail)
		{
			try
			{
				borrowDetail.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
				borrowDetail.CreatedDate = DateTime.Now;
				borrowDetail.IsDeleted = false;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/CreateLmsLiteratureBorrowDetail");
				var postBody = borrowDetail;
				request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				var response = await new HttpClient().SendAsync(request);

				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadFromJsonAsync<LmsLiteratureBorrowDetail>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
				else
				{
					var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}
		#endregion

		#region Update
		protected LmsLiteratureBorrowDetail UniqueLiteratureBorrowDetail = new();

		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master"> getting Literature BorrowDetail  by Id</History>
		public async Task<ApiCallResponse> GetLmsLiteratureBorrowDetailById(int id)
		{
			try
			{
				var response = await GetUniqueLmsLiteratureBorrowDetails(id);
				if (response.IsSuccessStatusCode)
				{
                    UniqueLiteratureBorrowDetail = (LmsLiteratureBorrowDetail)response.ResultData;
                    OnLmsLiteratureBorrowDetailGet(UniqueLiteratureBorrowDetail);
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = UniqueLiteratureBorrowDetail };
                }
				else
				{
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}
		partial void OnLmsLiteratureBorrowDetailGet(LmsLiteratureBorrowDetail item);
		protected LmsLiteratureBorrowDetail task = new();

		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master"> Call API for getting Literature BorrowDetail by Id</History>
		public async Task<ApiCallResponse> GetUniqueLmsLiteratureBorrowDetails(int Id)
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetLmsLiteratureBorrowDetailById?id=" + Id);
				// add authorization header
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				// send request
				var response = await new HttpClient().SendAsync(request);
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					throw new Exception(translationState.Translate("Contact_Administrator"));
				}
				else if (response.IsSuccessStatusCode)
				{
                    var result = await response.Content.ReadFromJsonAsync<LmsLiteratureBorrowDetail>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = result };
                }
				else
				{
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }

			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}

		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master"> Updating Literature BorrowDetail</History>
		public async Task<ApiCallResponse> UpdateLmsLiteratureBorrowDetail(int id, LmsLiteratureBorrowDetail lmsLiteratureBorrowDetail)
		{
			try
			{
				// OnLmsLiteratureBorrowDetailUpdated(lmsLiteratureBorrowDetail);
				var responsedata = await GetUniqueLmsLiteratureBorrowDetails(id);
				if (responsedata.IsSuccessStatusCode)
				{
					UniqueLiteratureBorrowDetail = (LmsLiteratureBorrowDetail)responsedata.ResultData;
                }
				else
				{
                    return responsedata;
                }
				if (UniqueLiteratureBorrowDetail == null)
				{
					throw new Exception(translationState.Translate("Item_Unavailable"));
				}
				var response = await UpdateLiteratureBorrowDetail(lmsLiteratureBorrowDetail);
				if (response.IsSuccessStatusCode)
				{
					return response;
				}
				else
				{
					return response;
				}
				// OnAfterLmsLiteratureBorrowDetailUpdated(lmsLiteratureBorrowDetail);
			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}


		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master">Call API for Updating Literature BorrowDetail</History>
		protected async Task<ApiCallResponse> UpdateLiteratureBorrowDetail(LmsLiteratureBorrowDetail item)
		{
			try
			{
				item.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");
				item.ModifiedDate = DateTime.Now;
				var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/UpdateLmsLiteratureBorrowDetail");
				var postBody = item;
				request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				var response = await new HttpClient().SendAsync(request);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadFromJsonAsync<LmsLiteratureBorrowDetail>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
				else
				{
					var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}

		#endregion

		#region Update Return book 
		//<History Author = 'Nabeel ur Rehman' Date='2022-03-22' Version="1.0" Branch="master"> Updating Literature Return Detail</History>
		public async Task<ApiCallResponse> UpdateLmsLiteratureReturnDetail(BorrowDetailVM lmsLiteratureBorrowDetail)
		{
			try
			{
				if (UniqueLiteratureBorrowDetail == null)
				{
					throw new Exception(translationState.Translate("Item_Unavailable"));
				}
				var response = await UpdateLiteratureReturnDetail(lmsLiteratureBorrowDetail);
				if (response.IsSuccessStatusCode)
				{
					return response;
				}
				else
				{
					return response;
				}
				// OnAfterLmsLiteratureBorrowDetailUpdated(lmsLiteratureBorrowDetail);


			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}

		//<History Author = 'Nabeel ur Rehman' Date='2022-03-22' Version="1.0" Branch="master">Call API for Updating Literature BorrowDetail</History>
		protected async Task<ApiCallResponse> UpdateLiteratureReturnDetail(BorrowDetailVM item)
		{
			try
			{
				item.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");
				item.ModifiedDate = DateTime.Now;
				var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/UpdateLmsLiteratureRetunDetail");
				var postBody = item;
				request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				var response = await new HttpClient().SendAsync(request);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadFromJsonAsync<BorrowDetailVM>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
				else
				{
					var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}

		#endregion

		#region Delete

		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master">Deleting Literature BorrowDetail</History>
		public async Task<ApiCallResponse> DeleteLmsLiteratureBorrowDetail(BorrowDetailVM item)
		{
			try
			{
				//UniqueLiteratureBorrowDetail = await GetUniqueLmsLiteratureBorrowDetails(id);

				//if (UniqueLiteratureBorrowDetail == null)
				//{
				//    throw new Exception(translationState.Translate("Item_Unavailable"));
				//}

				//OnLmsLiteratureBorrowDetailDeleted(UniqueLiteratureBorrowDetail);
				item.DeletedBy = await _browserStorage.GetItemAsync<string>("User");

				var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/DeleteLmsLiteratureBorrow");
				var postBody = item;
				request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				var response = await new HttpClient().SendAsync(request);

				//OnAfterLmsLiteratureBorrowDetailDeleted(UniqueLiteratureBorrowDetail);
				if (response.IsSuccessStatusCode)
				{
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = UniqueLiteratureBorrowDetail };
				}
				else
				{
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
				}
			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}
		//partial void OnLmsLiteratureBorrowDetailDeleted(LmsLiteratureBorrowDetail item);
		//partial void OnAfterLmsLiteratureBorrowDetailDeleted(LmsLiteratureBorrowDetail item);

		#endregion

		#region Import/Export
		public async Task ExportLmsLiteratureBorrowDetailsToCSV(Query query = null, string fileName = null)
		{
			navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsLiteratureborrowdetails/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsLiteratureborrowdetails/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
		}
		public async Task ExportLmsLiteratureBorrowDetailsToExcel(Query query = null, string fileName = null)
		{
			navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsLiteratureborrowdetails/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsLiteratureborrowdetails/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
		}
		#endregion

		#endregion

		#region Literature Borrow Extension Approval

		//<History Author = 'Zain Ul Islam' Date='2022-03-22' Version="1.0" Branch="master"> Get List of Literature Borrow Details</History>
		public async Task<IQueryable<BorrowDetailVM>> GetLiteratureBorrowExtensionApprovals(Query query = null)
		{
			try
			{
				var response = await GetLmsLiteratureBorrowExtensionApprovals();
				if (response.IsSuccessStatusCode)
				{
					var data = (IEnumerable<BorrowDetailVM>)response.ResultData;
					var items = data.AsQueryable();
					if (query != null)
					{
						if (!string.IsNullOrEmpty(query.Expand))
						{
							var propertiesToExpand = query.Expand.Split(',');
							foreach (var p in propertiesToExpand)
							{
								items = items.Include(p);
							}
						}

						if (!string.IsNullOrEmpty(query.Filter))
						{
							if (query.FilterParameters != null)
							{
								items = items.Where(query.Filter, query.FilterParameters);
							}
							else
							{
								items = items.Where(query.Filter);
							}
						}

						if (!string.IsNullOrEmpty(query.OrderBy))
						{
							items = items.OrderBy(query.OrderBy);
						}

						if (query.Skip.HasValue)
						{
							items = items.Skip(query.Skip.Value);
						}

						if (query.Top.HasValue)
						{
							items = items.Take(query.Top.Value);
						}
					}
					OnLmsLiteratureBorrowExtensionRead(ref items);
					return await Task.FromResult(items);
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		partial void OnLmsLiteratureBorrowExtensionRead(ref IQueryable<BorrowDetailVM> items);

		public async Task<ApiCallResponse> GetLmsLiteratureBorrowExtensionApprovals()
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetLmsLiteratureBorrowExtensionApprovals");
				// add authorization header
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				// send request
				var response = await new HttpClient().SendAsync(request);
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					throw new Exception(translationState.Translate("Contact_Administrator"));
				}
				else if (response.IsSuccessStatusCode)
				{
                    var responselist = response.Content.ReadFromJsonAsync<IEnumerable<BorrowDetailVM>>();
                    var queryableX = (await responselist).AsQueryable();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = queryableX };
                }
				else
				{
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
				}
            }
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}

		protected IEnumerable<LiteratureBorrowApprovalType> approvalTypes;

		public async Task<IEnumerable<LiteratureBorrowApprovalType>> getLiteratureBorrowApprovalTypes()
		{
			try
			{
				approvalTypes = await GetLiteratureBorrowApprovalTypes();
				return await Task.FromResult(approvalTypes);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private async Task<IEnumerable<LiteratureBorrowApprovalType>> GetLiteratureBorrowApprovalTypes()
		{
			try
			{
				return await new HttpClient().GetFromJsonAsync<IEnumerable<LiteratureBorrowApprovalType>>(_config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetLiteratureBorrowApprovalTypes");
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}


		}

		#endregion

		#region Literature Borrow Approval
		partial void OnLmsLiteratureBorrowApprovalRead(ref IQueryable<BorrowDetailVM> items);

		public async Task<ApiCallResponse> GetLiteratureBorrowApprovals()
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetLiteratureBorrowApprovals");
				// add authorization header
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				// send request
				var response = await new HttpClient().SendAsync(request);
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					throw new Exception(translationState.Translate("Contact_Administrator"));
				}
				if (response.IsSuccessStatusCode)
				{
                    var responselist = response.Content.ReadFromJsonAsync<IEnumerable<BorrowDetailVM>>();
                    var queryableX = (await responselist).AsQueryable();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = queryableX };
                }
				else
				{
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}
		#endregion

		#region Update

		public async Task<ApiCallResponse> UpdateLiteratureBorrowApprovalStatus(int id, LmsLiteratureBorrowDetail lmsLiteratureBorrowDetail)
		{
			try
			{

				var responsedata = await GetUniqueLmsLiteratureBorrowDetails(id);
				if (responsedata.IsSuccessStatusCode)
				{
					UniqueLiteratureBorrowDetail = (LmsLiteratureBorrowDetail)responsedata.ResultData;
                }
				if (UniqueLiteratureBorrowDetail == null)
				{
					throw new Exception(translationState.Translate("Item_Unavailable"));
				}
				var response = await UpdateLiteratureBorrowApprovalStatus(lmsLiteratureBorrowDetail);
				if (response.IsSuccessStatusCode)
				{
					return response;
				}
				else
				{
					return response;
				}

			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}

		protected async Task<ApiCallResponse> UpdateLiteratureBorrowApprovalStatus(LmsLiteratureBorrowDetail item)
		{
			try
			{
				item.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");
				item.ModifiedDate = DateTime.Now;
				var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/UpdateLiteratureBorrowApprovalStatus");
				var postBody = item;
				request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				var response = await new HttpClient().SendAsync(request);
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadFromJsonAsync<LmsLiteratureBorrowDetail>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
				else
				{
					var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
			}
			catch (Exception ex)
			{
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
		}


		#endregion

		#region Import/Export
		public async Task ExportLiteratureBorrowApprovalsToCSV(Query query = null, string fileName = null)
		{
			navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureborrowapprovals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteratureborrowapprovals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
		}
		public async Task ExportLiteratureBorrowApprovalsToExcel(Query query = null, string fileName = null)
		{
			navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureborrowapprovals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteratureborrowapprovals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
		}
		#endregion


		#region get isactive & isborrowable barcode number detail's by using literature id
		public async Task<ApiCallResponse> GetBarcodeNumberDetailByusingLiteratureId(int literatureId)
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetBarcodeNumberDetailByusingLiteratureId?literatureId=" + literatureId);
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
				var response = await new HttpClient().SendAsync(request);
				if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
				{
					var content = new LiteratureDetailsForBorrowRequestVM();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
				else if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadFromJsonAsync<LiteratureDetailsForBorrowRequestVM>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
				else
				{
					var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
					return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
				}
			}
			catch (Exception ex)
			{
				return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
			}
        }
        #endregion

        #region Get Borrow Approval Status Details
        public async Task<ApiCallResponse> GetBorrowApprovalStatusDetails()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetBorrowApprovalStatusDetails");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<LiteratureBorrowApprovalType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = result };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }

            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion
        #region Get User And Literature Detail By UserId And CivilId
        public async Task<ApiCallResponse> GetBorrowedLiteratureAndUserDetailByUserIdAndCivilId(string? civilId, string? Id ,string Token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetBorrowedLiteratureAndUserDetailByUserIdAndCivilId?UserId="+Id + "&civilId=" + civilId);
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserAndLiteratureVM>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = result };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }

            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

       
        public async Task<ApiCallResponse> UpdateLiteratureReturnExtendDetail(BorrowedLiteratureVM item)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/UpdateLiteratureReturnExtendDetail");
                var postBody = item;
                request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<BorrowedLiteratureVM>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        public async Task<ApiCallResponse> GetLiteratureByBarcode(string barCode)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetLiteratureByBarcode?barCode="+barCode);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));

                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BorrowedLiteratureVM>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = result };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        public async Task<ApiCallResponse> GetUserBorrowHistoryByUserId(string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetUserBorrowHistoryByUserId?userId=" + userId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));

                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<UserBorrowedHistoryVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = result };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        public async Task<ApiCallResponse> GetAllLmsUserList()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetAllLmsUserList");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<AllLmsUserDetailVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };

                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }

            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }

        }


        public async Task<ApiCallResponse> GetLmsBorrowLiteraturesAdvanceSearch(LiteratureAdvancedSearchVM advanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureBorrowDetails/GetLmsBorrowLiteraturesAdvanceSearch");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<BorrowedLiteratureVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }



    }
}
