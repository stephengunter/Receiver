using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSTrader;
using CSAPIComm;

namespace Core.Providers
{
    public class Onrich
    {
        private const string HOST = "stest.cathayfut.com.tw";
        private const ushort PORT = 8686;
        private const string SOURCE = "API";
        private const string PRODUCT_URL = "stest.cathayfut.com.tw:8687";
        private TradeAPI _tradeAPI;

        private bool _tradeLoginOK = false;

        public Onrich(string sid, string password)
        {
            _tradeAPI = new TradeAPI(HOST, PORT, SOURCE)
            {
                AutoRetrieveProductInfo = true,
                AutoSubReport = true,
                AutoRecoverReport = true                 
            };

            _tradeAPI.SetProductFileBaseURL(PRODUCT_URL);

            _tradeAPI.OnTradeAPIRcvData += OnTAPIRcvData;
            _tradeAPI.OnTradeAPIStatus += OnTAPIStatus;
        }

        public void Connect()
        {
            int result = _tradeAPI.Connect();
            if (result < 0) OnConnectFailed(result);
            else
            {
                //連線成功
                string msg = "連線成功.";
            }
        }

        public void Disconnect()
        {
            _tradeAPI.Disconnect();
        }

        void OnConnectFailed(int code)
        {
            string msg = "連線失敗. ";
            if (code == -1)
            {
                msg += $"主機連線中 code: {code}";
            }
            else if (code == -2)
            {
                msg += $"主機已連線 code: {code}";
            }
            else if (code == -3)
            {
                msg += $"找不到對應的主機或通訊埠 code: {code}";
            }
        }

        void LoginTrade(string account, string password, string subAccount)
        {
            //tradeAPI.AutoRetrieveProductInfo = chkAutoLoadProduct.Checked;
            //tradeAPI.AutoSubReport = chkSubReports.Checked;
            //tradeAPI.AutoRecoverReport = chkRecoverReports.Checked;
            //if (chkAutoLoadProduct.Checked) tradeAPI.SetProductFileBaseURL(cbxProductURL.Text);
            _tradeAPI.LoginTrade(account, password, subAccount);
        }

        #region Event Handlers
        private void OnTAPIRcvData(TradeAPI sender, MomBase mb)
        {
            string msg = "";
            int errorCode = 0;
            switch (mb.dataType)
            {
                case 103:
                    MB103 mb103 = mb as MB103;
                    msg = String.Format("公告查詢成功，數量[{0}]", mb103.notices.Count);
                    foreach (NoticeMsg item in mb103.notices)
                    {
                        msg += Environment.NewLine;
                        msg += String.Format("{0}|{1}|{2}|{3}", item.kind, item.notice_type, item.post_time, item.content);                        
                    }
                    break;
                case 114:
                    MB114 mb114 = mb as MB114;
                    msg = String.Format(" 收到主機公告, {0}", mb114.content);
                    break;
                case 201:
                    MB201 mb201 = mb as MB201;
                    if (int.TryParse(mb201.err_code, out errorCode) && (errorCode == 0))
                    {
                        msg = String.Format("{0} 委託成功 {1}", mb201.user_def, mb201.toLog());
                    }
                    else
                    {
                        msg = String.Format("{0} 委託失敗, ErrCode={1} ErrMsg={2}", mb201.user_def, mb201.err_code, mb201.err_msg);
                    }
                    break;
                case 202:
                    MB202 mb202 = mb as MB202;
                    msg = String.Format("{0} 成交{1}口 {2}", mb202.user_def, mb202.qty_cum, mb202.toLog());
                    break;
                case 401:
                    MB401 mb401 = mb as MB401;
                    if (int.TryParse(mb401.err_code, out errorCode) && (errorCode == 0))
                    {
                        msg = String.Format("{0} 證券委託成功 {1}", mb401.user_def, mb401.toLog());
                    }
                    else
                    {
                        msg = String.Format("{0} 證券委託失敗, ErrCode={1} ErrMsg={2}", mb401.user_def, mb401.err_code, mb401.err_msg);
                    }
                    break;
                case 402:
                    MB402 mb402 = mb as MB402;
                    msg = String.Format("{0} 成交{1}口 {2}", mb402.user_def, mb402.deal_qty, mb402.toLog());
                    break;
                case 404:
                case 405:
                case 406:
                    msg = String.Format(" 委託回報回補:{0}", mb.toLog());
                    break;
                case 411:
                    msg = String.Format(" 即時庫存查詢:{0}", mb.toLog());
                    break;
                case 413:
                    msg = String.Format(" 對帳單查詢:{0}", mb.toLog());
                    break;
                case 415:
                    msg = String.Format(" 整戶維持率查詢:{0}", mb.toLog());
                    break;
                case 417:
                    msg = String.Format(" 證券庫存回報:{0}", mb.toLog());
                    break;
                case 419:
                    msg = String.Format(" 證券全額交割股回覆:{0}", mb.toLog());
                    break;
                case 421:
                    msg = String.Format(" 證券歷史淨收付回報:{0}", mb.toLog());
                    break;
                case 423:
                    msg = String.Format(" 證券對帳單回報:{0}", mb.toLog());
                    break;
                case 425:
                    msg = String.Format(" 證券已實現損益查詢:{0}", mb.toLog());
                    break;
                case 427:
                    msg = String.Format(" 證券即時庫存明細損益試算回報:{0}", mb.toLog());
                    break;
                case 429:
                    msg = String.Format(" 證券即時庫存彙總損益試算回報:{0}", mb.toLog());
                    break;
                case 431:
                    msg = String.Format(" 證券自訂成本回覆:{0}", mb.toLog());
                    break;
                case 205:
                    MB205 mb205 = mb as MB205;
                    if (mb205.sub_acno.Trim().Length > 0)
                    {
                        msg = String.Format("{0}-{1} 委託回報回補 {2}", mb205.branch_id, mb205.sub_acno, mb205.toLog());
                    }
                    else
                    {
                        msg = String.Format("\r\n{0}-{1} 委託回報回補 {2}", mb205.branch_id, mb205.acno, mb205.toLog());
                    }
                    break;
                case 206:
                    MB206 mb206 = mb as MB206;
                    if (mb206.sub_acno.Trim().Length > 0)
                    {
                        msg = String.Format("\r\n{0}-{1} 成交回報回補 {2}", mb206.branch_id, mb206.sub_acno, mb206.toLog());
                    }
                    else
                    {
                        msg = String.Format("\r\n{0}-{1} 成交回報回補 {2}", mb206.branch_id, mb206.acno, mb206.toLog());
                    }
                    break;
                //case 301:
                case 328:
                    MB328 mb328 = mb as MB328;
                    msg = String.Format(" 權益數查詢成功:{0}", mb328.toLog());
                    break;
                case 303:
                    MB303 mb303 = mb as MB303;
                    msg = String.Format(" 客戶部位明細查詢成功:{0}", mb303.toLog());
                    break;
                case 305:
                    MB305 mb305 = mb as MB305;
                    msg = String.Format(" 客戶平倉明細查詢成功:{0}", mb305.toLog());
                    break;
                case 308:
                    MB308 mb308 = mb as MB308;
                    msg = String.Format(" 客戶平倉彙總查詢成功:{0}", mb308.toLog());
                    break;
                case 310:
                    MB310 mb310 = mb as MB310;
                    msg = String.Format(" 客戶部位彙總查詢成功:{0}", mb310.toLog());
                    break;
                case 312:
                    MB312 mb312 = mb as MB312;
                    msg = String.Format(" 客戶部位明細查詢成功:{0}", mb312.toLog());
                    break;
                case 229:
                    MB229 mb229 = mb as MB229;
                    msg = String.Format(" 客戶平倉明細MB229查詢成功:{0}", mb229.toLog());
                    break;
                case 314:
                case 326:
                    MB314 mb314 = mb as MB314;
                    msg = String.Format(" 客戶平倉明細彙總查詢成功:{0}", mb314.toLog());
                    break;
                case 316:
                    MB316 mb316 = mb as MB316;
                    msg = String.Format(" 客戶部位彙總查詢成功:{0}", mb316.toLog());
                    break;
                case 320:
                case 323:
                    MB320 mb320 = mb as MB320;
                    msg = String.Format(" VIP權益數查詢成功:{0}", mb320.toLog());
                    break;
                case 322:
                case 324:
                    MB322 mb322 = mb as MB322;
                    msg = String.Format(" VIP部位彙總查詢成功:{0}", mb322.toLog());
                    break;
                case 501:
                case 505:
                    MB501 mb501 = mb as MB501;
                    if (int.TryParse(mb501.ErrCode, out errorCode) && (errorCode == 0))
                    {
                        msg = String.Format("{0} 委託成功 {1}", mb501.UserDef, mb501.toLog());
                    }
                    else
                    {
                        msg = String.Format("{0} 委託失敗, ErrCode={1} ErrMsg={2}", mb501.UserDef, mb501.ErrCode, mb501.Msg);
                    }
                    break;
                case 502:
                case 506:
                    MB502 mb502 = mb as MB502;
                    msg = String.Format("成交{0}口 {1}", mb502.DealQty, mb502.toLog());
                    break;
                case 504:
                    MB504 mb504 = mb as MB504;
                    msg = String.Format(" 外期回補回報狀態通知:{0}", mb504.toLog());
                    break;
                case 511:
                    MB511 mb511 = mb as MB511;
                    msg = String.Format(" 外期權益數查詢成功(大量):{0}", mb511.toLog());
                    break;
                case 513:
                    MB513 mb513 = mb as MB513;
                    msg = String.Format(" 外期部位彙總查詢成功(大量):{0}", mb513.toLog());
                    break;
                case 515:
                    MB515 mb515 = mb as MB515;
                    msg = String.Format(" 外期部位明細查詢成功(大量):{0}", mb515.toLog());
                    break;
                case 517:
                    MB517 mb517 = mb as MB517;
                    msg = String.Format(" 外期平倉明細查詢成功(大量):{0}", mb517.toLog());
                    break;
                case 9000:
                    msg = String.Format(" Note for market status:{0}", mb.toLog());
                    break;
                default:
                    break;
            }
        }

        private void OnTAPIStatus(object sender, MESSAGE_TYPE status, string msg)
        {
            if (String.IsNullOrEmpty(msg)) msg = "";
            else msg= msg.Trim();

            string text = "";
            switch (status)
            {
                case MESSAGE_TYPE.MT_CONNECT_READY:  // 連線成功
                    text = String.Format("MT_CONNECT_READY:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_CONNECT_FAIL: // 連線錯誤
                    text = String.Format("MT_CONNECT_FAIL:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_DISCONNECTED: // 已斷線
                    text = String.Format("MT_DISCONNECTED:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_SUBSCRIBE: // 訂閱
                    text = String.Format("MT_SUBSCRIBE:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_UNSUBSCRIBE: // 解除訂閱
                    text = String.Format("MT_UNSUBSCRIBE:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_HEART_BEAT:
                    text = String.Format("MT_HEART_BEAT:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_LOGIN_OK: // 登入成功
                    _tradeLoginOK = true;
                    text = String.Format("MT_LOGIN_OK:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_LOGIN_FAIL: // 登入失敗
                    text = String.Format("MT_LOGIN_FAIL:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_ERROR:
                    text = String.Format("MT_ERROR:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_RETRIEVE_FUT_DONE:
                    text = String.Format("MT_RETRIEVE_FUT_DONE:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_RETRIEVE_OPT_DONE:
                    text = String.Format("MT_RETRIEVE_OPT_DONE:{0}", msg);
                    break;
                case MESSAGE_TYPE.MT_RETRIEVE_STK_DONE:
                    text = String.Format("MT_RETRIEVE_STK_DONE:{0}", msg);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
