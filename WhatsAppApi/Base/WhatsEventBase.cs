﻿using System;
using System.Collections.Generic;
using WhatsAppApi.Helper;
using WhatsAppApi.Response;

namespace WhatsAppApi
{
    public class WhatsEventBase : ApiBase
    {
        //events
        public event ExceptionDelegate OnDisconnect;
        protected void fireOnDisconnect(Exception ex)
        {
            if (OnDisconnect != null)
            {
                OnDisconnect(ex);
            }
        }
        
        public event NullDelegate OnConnectSuccess;
        protected void fireOnConnectSuccess() 
        {
            if (OnConnectSuccess != null)
            {
                OnConnectSuccess();
            }
        }
        
        public event ExceptionDelegate OnConnectFailed;
        protected void fireOnConnectFailed(Exception ex) 
        {
            if (OnConnectFailed != null)
            {
                OnConnectFailed(ex);
            }
        }

        public event LoginSuccessDelegate OnLoginSuccess;
        protected void fireOnLoginSuccess(string pn, byte[] data) 
        {
            if (OnLoginSuccess != null)
            {
                OnLoginSuccess(pn, data);
            }
        }

        public event StringDelegate OnLoginFailed;
        protected void fireOnLoginFailed(string data) 
        {
            if (OnLoginFailed != null)
            {
                OnLoginFailed(data);
            }
        }

        public event OnGetMessageDelegate OnGetMessage;
        protected void fireOnGetMessage(ProtocolTreeNode messageNode, string from, string id, string name, string message, bool receipt_sent)
        {
            if (OnGetMessage != null)
            {
                OnGetMessage(messageNode, from, id, name, message, receipt_sent);
            }
        }

        public event OnGetMediaDelegate OnGetMessageImage;
        protected void fireOnGetMessageImage(ProtocolTreeNode mediaNode, string from, string id, string fileName, int fileSize, string url, byte[] preview)
        {
            if (OnGetMessageImage != null)
            {
                OnGetMessageImage(mediaNode, from, id, fileName, fileSize, url, preview);
            }
        }

        public event OnGetMediaDelegate OnGetMessageVideo;
        protected void fireOnGetMessageVideo(ProtocolTreeNode mediaNode, string from, string id, string fileName, int fileSize, string url, byte[] preview)
        {
            if (OnGetMessageVideo != null)
            {
                OnGetMessageVideo(mediaNode, from, id, fileName, fileSize, url, preview);
            }
        }

        public event OnGetMediaDelegate OnGetMessageAudio;
        protected void fireOnGetMessageAudio(ProtocolTreeNode mediaNode, string from, string id, string fileName, int fileSize, string url, byte[] preview)
        {
            if (OnGetMessageAudio != null)
            {
                OnGetMessageAudio(mediaNode, from, id, fileName, fileSize, url, preview);
            }
        }

        public event OnGetLocationDelegate OnGetMessageLocation;
        protected void fireOnGetMessageLocation(ProtocolTreeNode locationNode, string from, string id, double lon, double lat, string url, string name, byte[] preview)
        {
            if (OnGetMessageLocation != null)
            {
                OnGetMessageLocation(locationNode, from, id, lon, lat, url, name, preview);
            }
        }

        public event OnGetVcardDelegate OnGetMessageVcard;
        protected void fireOnGetMessageVcard(ProtocolTreeNode vcardNode, string from, string id, string name, byte[] data)
        {
            if (OnGetMessageVcard != null)
            {
                OnGetMessageVcard(vcardNode, from, id, name, data);
            }
        }

        public event OnErrorDelegate OnError;
        protected void fireOnError(string id, string from, int code, string text)
        {
            if (OnError != null)
            {
                OnError(id, from, code, text);
            }
        }

        public event OnNotificationPictureDelegate OnNotificationPicture;
        protected void fireOnNotificationPicture(string type, string jid, string id)
        {
            if (OnNotificationPicture != null)
            {
                OnNotificationPicture(type, jid, id);
            }
        }

        public event OnGetMessageReceivedDelegate OnGetMessageReceivedServer;
        protected void fireOnGetMessageReceivedServer(string from, string id)
        {
            if (OnGetMessageReceivedServer != null)
            {
                OnGetMessageReceivedServer(from, id);
            }
        }

        public event OnGetMessageReceivedDelegate OnGetMessageReceivedClient;
        protected void fireOnGetMessageReceivedClient(string from, string id)
        {
            if (OnGetMessageReceivedClient != null)
            {
                OnGetMessageReceivedClient(from, id);
            }
        }

        public event OnGetPresenceDelegate OnGetPresence;
        protected void fireOnGetPresence(string from, string type)
        {
            if (OnGetPresence != null)
            {
                OnGetPresence(from, type);
            }
        }

        public event OnGetGroupParticipantsDelegate OnGetGroupParticipants;
        protected void fireOnGetGroupParticipants(string gjid, string[] jids)
        {
            if (OnGetGroupParticipants != null)
            {
                OnGetGroupParticipants(gjid, jids);
            }
        }

        public event OnGetLastSeenDelegate OnGetLastSeen;
        protected void fireOnGetLastSeen(string from, DateTime lastSeen)
        {
            if (OnGetLastSeen != null)
            {
                OnGetLastSeen(from, lastSeen);
            }
        }

        public event OnGetChatStateDelegate OnGetTyping;
        protected void fireOnGetTyping(string from)
        {
            if (OnGetTyping != null)
            {
                OnGetTyping(from);
            }
        }

        public event OnGetChatStateDelegate OnGetPaused;
        protected void fireOnGetPaused(string from)
        {
            if (OnGetPaused != null)
            {
                OnGetPaused(from);
            }
        }

        public event OnGetPictureDelegate OnGetPhoto;
        protected void fireOnGetPhoto(string from, string id, byte[] data)
        {
            if (OnGetPhoto != null)
            {
                OnGetPhoto(from, id, data);
            }
        }

        public event OnGetPictureDelegate OnGetPhotoPreview;
        protected void fireOnGetPhotoPreview(string from, string id, byte[] data)
        {
            if (OnGetPhotoPreview != null)
            {
                OnGetPhotoPreview(from, id, data);
            }
        }

        public event OnGetGroupsDelegate OnGetGroups;
        protected void fireOnGetGroups(WaGroupInfo[] groups)
        {
            if (OnGetGroups != null)
            {
                OnGetGroups(groups);
            }
        }

        public event OnContactNameDelegate OnGetContactName;
        protected void fireOnGetContactName(string from, string contactName)
        {
            if (OnGetContactName != null)
            {
                OnGetContactName(from, contactName);
            }
        }

        public event OnGetStatusDelegate OnGetStatus;
        protected void fireOnGetStatus(string from, string type, string name, string status)
        {
            if (OnGetStatus != null)
            {
                OnGetStatus(from, type, name, status);
            }
        }

        public event OnGetSyncResultDelegate OnGetSyncResult;
        protected void fireOnGetSyncResult(int index, string sid, Dictionary<string, string> existingUsers, string[] failedNumbers)
        {
            if (OnGetSyncResult != null)
            {
                OnGetSyncResult(index, sid, existingUsers, failedNumbers);
            }
        }

        public event OnGetPrivacySettingsDelegate OnGetPrivacySettings;
        protected void fireOnGetPrivacySettings(Dictionary<VisibilityCategory, VisibilitySetting> settings)
        {
            if (OnGetPrivacySettings != null)
            {
                OnGetPrivacySettings(settings);
            }
        }

        public event OnGetParticipantAddedDelegate OnGetParticipantAdded;
        protected void fireOnGetParticipantAdded(string gjid, string jid, DateTime time)
        {
            if (OnGetParticipantAdded != null)
            {
                OnGetParticipantAdded(gjid, jid, time);
            }
        }

        public event OnGetParticipantRemovedDelegate OnGetParticipantRemoved;
        protected void fireOnGetParticipantRemoved(string gjid, string jid, string author, DateTime time)
        {
            if (OnGetParticipantRemoved != null)
            {
                OnGetParticipantRemoved(gjid, jid, author, time);
            }
        }

        public event OnGetParticipantRenamedDelegate OnGetParticipantRenamed;
        protected void fireOnGetParticipantRenamed(string gjid, string oldJid, string newJid, DateTime time)
        {
            if (OnGetParticipantRenamed != null)
            {
                OnGetParticipantRenamed(gjid, oldJid, newJid, time);
            }
        }

        public event OnGetGroupSubjectDelegate OnGetGroupSubject;
        protected void fireOnGetGroupSubject(string gjid, string jid, string username, string subject, DateTime time)
        {
            if (OnGetGroupSubject != null)
            {
                OnGetGroupSubject(gjid, jid, username, subject, time);
            }
        }

        //event delegates
        public delegate void OnContactNameDelegate(string from, string contactName);
        public delegate void NullDelegate();
        public delegate void ExceptionDelegate(Exception ex);
        public delegate void LoginSuccessDelegate(string phoneNumber, byte[] data);
        public delegate void StringDelegate(string data);
        public delegate void OnErrorDelegate(string id, string from, int code, string text);
        public delegate void OnGetMessageReceivedDelegate(string from, string id);
        public delegate void OnNotificationPictureDelegate(string type, string jid, string id);
        public delegate void OnGetMessageDelegate(ProtocolTreeNode messageNode, string from, string id, string name, string message, bool receipt_sent);
        public delegate void OnGetPresenceDelegate(string from, string type);
        public delegate void OnGetGroupParticipantsDelegate(string gjid, string[] jids);
        public delegate void OnGetLastSeenDelegate(string from, DateTime lastSeen);
        public delegate void OnGetChatStateDelegate(string from);
        public delegate void OnGetMediaDelegate(ProtocolTreeNode mediaNode, string from, string id, string fileName, int fileSize, string url, byte[] preview);
        public delegate void OnGetLocationDelegate(ProtocolTreeNode locationNode, string from, string id, double lon, double lat, string url, string name, byte[] preview);
        public delegate void OnGetVcardDelegate(ProtocolTreeNode vcardNode, string from, string id, string name, byte[] data);
        public delegate void OnGetPictureDelegate(string from, string id, byte[] data);
        public delegate void OnGetGroupsDelegate(WaGroupInfo[] groups);
        public delegate void OnGetStatusDelegate(string from, string type, string name, string status);
        public delegate void OnGetSyncResultDelegate(int index, string sid, Dictionary<string, string> existingUsers, string[] failedNumbers);
        public delegate void OnGetPrivacySettingsDelegate(Dictionary<VisibilityCategory, VisibilitySetting> settings);
        public delegate void OnGetParticipantAddedDelegate(string gjid, string jid, DateTime time);
        public delegate void OnGetParticipantRemovedDelegate(string gjid, string jid, string author, DateTime time);
        public delegate void OnGetParticipantRenamedDelegate(string gjid, string oldJid, string newJid, DateTime time);
        public delegate void OnGetGroupSubjectDelegate(string gjid, string jid, string username, string subject, DateTime time);
    }
}
