Total operations: 186
Typed in v1: 186
Raw-only: 0

| Domain | Total | Typed v1 | Raw-only |
|---|---|---|---|
| Devices/H.323 | 17 | 17 | 0 |
| Live Meeting Controls | 4 | 4 | 0 |
| Meeting Summaries | 4 | 4 | 0 |
| Meetings core | 27 | 27 | 0 |
| Polls | 7 | 7 | 0 |
| Recordings/Archiving | 23 | 23 | 0 |
| Registrants | 8 | 8 | 0 |
| Reports | 24 | 24 | 0 |
| SIP Phones | 4 | 4 | 0 |
| TSP | 8 | 8 | 0 |
| Templates | 2 | 2 | 0 |
| Tracking Fields | 5 | 5 | 0 |
| Webinars | 53 | 53 | 0 |


### Devices/H.323
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/devices` | listDevices | Typed |
| POST | `/devices` | addDevice | Typed |
| GET | `/devices/groups` | Getzdmgroupinfo | Typed |
| POST | `/devices/zpa/assignment` | Assigndevicetoauser/commonarea | Typed |
| GET | `/devices/zpa/settings` | GetZpaDeviceListProfileSettingOfaUser | Typed |
| POST | `/devices/zpa/upgrade` | UpgradeZpas/app | Typed |
| DELETE | `/devices/zpa/vendors/{vendor}/mac_addresses/{macAddress}` | DeleteZpaDeviceByVendorAndMacAddress | Typed |
| GET | `/devices/zpa/zdm_groups/{zdmGroupId}/versions` | GetZpaVersioninfo | Typed |
| DELETE | `/devices/{deviceId}` | deleteDevice | Typed |
| GET | `/devices/{deviceId}` | getDevice | Typed |
| PATCH | `/devices/{deviceId}` | updateDevice | Typed |
| PATCH | `/devices/{deviceId}/assign_group` | assginGroup | Typed |
| PATCH | `/devices/{deviceId}/assignment` | changeDeviceAssociation | Typed |
| GET | `/h323/devices` | deviceList | Typed |
| POST | `/h323/devices` | deviceCreate | Typed |
| DELETE | `/h323/devices/{deviceId}` | deviceDelete | Typed |
| PATCH | `/h323/devices/{deviceId}` | deviceUpdate | Typed |

### Live Meeting Controls
| Method | Path | operationId | v1 status |
|---|---|---|---|
| DELETE | `/live_meetings/{meetingId}/chat/messages/{messageId}` | deleteMeetingChatMessageById | Typed |
| PATCH | `/live_meetings/{meetingId}/chat/messages/{messageId}` | updateMeetingChatMessageById | Typed |
| PATCH | `/live_meetings/{meetingId}/events` | inMeetingControl | Typed |
| PATCH | `/live_meetings/{meetingId}/rtms_app/status` | meetingRTMSStatusUpdate | Typed |

### Meeting Summaries
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/meetings/meeting_summaries` | Listmeetingsummaries | Typed |
| DELETE | `/meetings/{meetingId}/meeting_summary` | Deletemeetingorwebinarsummary | Typed |
| GET | `/meetings/{meetingId}/meeting_summary` | Getameetingsummary | Typed |
| GET | `/users/{userId}/meeting_summaries` | ListUserMeetingSummaries | Typed |

### Meetings core
| Method | Path | operationId | v1 status |
|---|---|---|---|
| DELETE | `/meetings/{meetingId}` | meetingDelete | Typed |
| GET | `/meetings/{meetingId}` | meeting | Typed |
| PATCH | `/meetings/{meetingId}` | meetingUpdate | Typed |
| GET | `/meetings/{meetingId}/invitation` | meetingInvitation | Typed |
| POST | `/meetings/{meetingId}/invite_links` | meetingInviteLinksCreate | Typed |
| GET | `/meetings/{meetingId}/jointoken/live_streaming` | meetingLiveStreamingJoinToken | Typed |
| GET | `/meetings/{meetingId}/jointoken/local_archiving` | meetingLocalArchivingArchiveToken | Typed |
| GET | `/meetings/{meetingId}/jointoken/local_recording` | meetingLocalRecordingJoinToken | Typed |
| GET | `/meetings/{meetingId}/livestream` | getMeetingLiveStreamDetails | Typed |
| PATCH | `/meetings/{meetingId}/livestream` | meetingLiveStreamUpdate | Typed |
| PATCH | `/meetings/{meetingId}/livestream/status` | meetingLiveStreamStatusUpdate | Typed |
| DELETE | `/meetings/{meetingId}/open_apps` | meetingAppDelete | Typed |
| POST | `/meetings/{meetingId}/open_apps` | meetingAppAdd | Typed |
| POST | `/meetings/{meetingId}/sip_dialing` | getSipDialingWithPasscode | Typed |
| PUT | `/meetings/{meetingId}/status` | meetingStatus | Typed |
| DELETE | `/meetings/{meetingId}/survey` | meetingSurveyDelete | Typed |
| GET | `/meetings/{meetingId}/survey` | meetingSurveyGet | Typed |
| PATCH | `/meetings/{meetingId}/survey` | meetingSurveyUpdate | Typed |
| GET | `/meetings/{meetingId}/token` | meetingToken | Typed |
| GET | `/past_meetings/{meetingId}` | pastMeetingDetails | Typed |
| GET | `/past_meetings/{meetingId}/instances` | pastMeetings | Typed |
| GET | `/past_meetings/{meetingId}/participants` | pastMeetingParticipants | Typed |
| GET | `/past_meetings/{meetingId}/qa` | listPastMeetingQA | Typed |
| GET | `/users/{userId}/meetings` | meetings | Typed |
| POST | `/users/{userId}/meetings` | meetingCreate | Typed |
| GET | `/users/{userId}/pac` | userPACs | Typed |
| GET | `/users/{userId}/upcoming_meetings` | listUpcomingMeeting | Typed |

### Polls
| Method | Path | operationId | v1 status |
|---|---|---|---|
| POST | `/meetings/{meetingId}/batch_polls` | createBatchPolls | Typed |
| GET | `/meetings/{meetingId}/polls` | meetingPolls | Typed |
| POST | `/meetings/{meetingId}/polls` | meetingPollCreate | Typed |
| DELETE | `/meetings/{meetingId}/polls/{pollId}` | meetingPollDelete | Typed |
| GET | `/meetings/{meetingId}/polls/{pollId}` | meetingPollGet | Typed |
| PUT | `/meetings/{meetingId}/polls/{pollId}` | meetingPollUpdate | Typed |
| GET | `/past_meetings/{meetingId}/polls` | listPastMeetingPolls | Typed |

### Recordings/Archiving
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/archive_files` | listArchivedFiles | Typed |
| GET | `/archive_files/download_audit` | listArchiveFileDownloadAudit | Typed |
| GET | `/archive_files/statistics` | getArchivedFileStatistics | Typed |
| PATCH | `/archive_files/{fileId}` | updateArchivedFile | Typed |
| DELETE | `/meetings/{meetingId}/recordings` | recordingDelete | Typed |
| GET | `/meetings/{meetingId}/recordings` | recordingGet | Typed |
| GET | `/meetings/{meetingId}/recordings/analytics_details` | analytics_details | Typed |
| GET | `/meetings/{meetingId}/recordings/analytics_summary` | analytics_summary | Typed |
| GET | `/meetings/{meetingId}/recordings/registrants` | meetingRecordingRegistrants | Typed |
| POST | `/meetings/{meetingId}/recordings/registrants` | meetingRecordingRegistrantCreate | Typed |
| GET | `/meetings/{meetingId}/recordings/registrants/questions` | recordingRegistrantsQuestionsGet | Typed |
| PATCH | `/meetings/{meetingId}/recordings/registrants/questions` | recordingRegistrantQuestionUpdate | Typed |
| PUT | `/meetings/{meetingId}/recordings/registrants/status` | meetingRecordingRegistrantStatus | Typed |
| GET | `/meetings/{meetingId}/recordings/settings` | recordingSettingUpdate | Typed |
| PATCH | `/meetings/{meetingId}/recordings/settings` | recordingSettingsUpdate | Typed |
| DELETE | `/meetings/{meetingId}/recordings/{recordingId}` | recordingDeleteOne | Typed |
| PUT | `/meetings/{meetingId}/recordings/{recordingId}/status` | recordingStatusUpdateOne | Typed |
| DELETE | `/meetings/{meetingId}/transcript` | DeleteMeetingTranscript | Typed |
| GET | `/meetings/{meetingId}/transcript` | GetMeetingTranscript | Typed |
| PUT | `/meetings/{meetingUUID}/recordings/status` | recordingStatusUpdate | Typed |
| DELETE | `/past_meetings/{meetingUUID}/archive_files` | deleteArchivedFiles | Typed |
| GET | `/past_meetings/{meetingUUID}/archive_files` | getArchivedFiles | Typed |
| GET | `/users/{userId}/recordings` | recordingsList | Typed |

### Registrants
| Method | Path | operationId | v1 status |
|---|---|---|---|
| POST | `/meetings/{meetingId}/batch_registrants` | addBatchRegistrants | Typed |
| GET | `/meetings/{meetingId}/registrants` | meetingRegistrants | Typed |
| POST | `/meetings/{meetingId}/registrants` | meetingRegistrantCreate | Typed |
| GET | `/meetings/{meetingId}/registrants/questions` | meetingRegistrantsQuestionsGet | Typed |
| PATCH | `/meetings/{meetingId}/registrants/questions` | meetingRegistrantQuestionUpdate | Typed |
| PUT | `/meetings/{meetingId}/registrants/status` | meetingRegistrantStatus | Typed |
| DELETE | `/meetings/{meetingId}/registrants/{registrantId}` | meetingregistrantdelete | Typed |
| GET | `/meetings/{meetingId}/registrants/{registrantId}` | meetingRegistrantGet | Typed |

### Reports
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/report/activities` | reportSignInSignOutActivities | Typed |
| GET | `/report/billing` | getBillingReport | Typed |
| GET | `/report/billing/invoices` | getBillingInvoicesReports | Typed |
| GET | `/report/cloud_recording` | reportCloudRecording | Typed |
| GET | `/report/daily` | reportDaily | Typed |
| GET | `/report/disclaimer` | Getdisclaimerreport | Typed |
| GET | `/report/history_meetings` | Gethistorymeetingandwebinarlist | Typed |
| GET | `/report/meeting_activities` | reportMeetingactivitylogs | Typed |
| GET | `/report/meetings/{meetingId}` | reportMeetingDetails | Typed |
| GET | `/report/meetings/{meetingId}/participants` | reportMeetingParticipants | Typed |
| GET | `/report/meetings/{meetingId}/polls` | reportMeetingPolls | Typed |
| GET | `/report/meetings/{meetingId}/qa` | reportMeetingQA | Typed |
| GET | `/report/meetings/{meetingId}/survey` | reportMeetingSurvey | Typed |
| GET | `/report/operationlogs` | reportOperationLogs | Typed |
| GET | `/report/remote_support` | Getremotesupportreport | Typed |
| GET | `/report/telephone` | reportTelephone | Typed |
| GET | `/report/upcoming_events` | reportUpcomingEvents | Typed |
| GET | `/report/users` | reportUsers | Typed |
| GET | `/report/users/{userId}/meetings` | reportMeetings | Typed |
| GET | `/report/webinars/{webinarId}` | reportWebinarDetails | Typed |
| GET | `/report/webinars/{webinarId}/participants` | reportWebinarParticipants | Typed |
| GET | `/report/webinars/{webinarId}/polls` | reportWebinarPolls | Typed |
| GET | `/report/webinars/{webinarId}/qa` | reportWebinarQA | Typed |
| GET | `/report/webinars/{webinarId}/survey` | reportWebinarSurvey | Typed |

### SIP Phones
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/sip_phones/phones` | ListSIPPhonePhones | Typed |
| POST | `/sip_phones/phones` | EnableSIPPhonePhones | Typed |
| DELETE | `/sip_phones/phones/{phoneId}` | deleteSIPPhonePhones | Typed |
| PATCH | `/sip_phones/phones/{phoneId}` | UpdateSIPPhonePhones | Typed |

### TSP
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/tsp` | tsp | Typed |
| PATCH | `/tsp` | tspUpdate | Typed |
| GET | `/users/{userId}/tsp` | userTSPs | Typed |
| POST | `/users/{userId}/tsp` | userTSPCreate | Typed |
| PATCH | `/users/{userId}/tsp/settings` | tspUrlUpdate | Typed |
| DELETE | `/users/{userId}/tsp/{tspId}` | userTSPDelete | Typed |
| GET | `/users/{userId}/tsp/{tspId}` | userTSP | Typed |
| PATCH | `/users/{userId}/tsp/{tspId}` | userTSPUpdate | Typed |

### Templates
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/users/{userId}/meeting_templates` | listMeetingTemplates | Typed |
| POST | `/users/{userId}/meeting_templates` | meetingTemplateCreate | Typed |

### Tracking Fields
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/tracking_fields` | trackingfieldList | Typed |
| POST | `/tracking_fields` | trackingfieldCreate | Typed |
| DELETE | `/tracking_fields/{fieldId}` | trackingfieldDelete | Typed |
| GET | `/tracking_fields/{fieldId}` | trackingfieldGet | Typed |
| PATCH | `/tracking_fields/{fieldId}` | trackingfieldUpdate | Typed |

### Webinars
| Method | Path | operationId | v1 status |
|---|---|---|---|
| DELETE | `/live_webinars/{webinarId}/chat/messages/{messageId}` | deleteWebinarChatMessageById | Typed |
| GET | `/past_webinars/{webinarId}/absentees` | webinarAbsentees | Typed |
| GET | `/past_webinars/{webinarId}/instances` | pastWebinars | Typed |
| GET | `/past_webinars/{webinarId}/participants` | listWebinarParticipants | Typed |
| GET | `/past_webinars/{webinarId}/polls` | listPastWebinarPollResults | Typed |
| GET | `/past_webinars/{webinarId}/qa` | listPastWebinarQA | Typed |
| GET | `/users/{userId}/webinar_templates` | listWebinarTemplates | Typed |
| POST | `/users/{userId}/webinar_templates` | webinarTemplateCreate | Typed |
| GET | `/users/{userId}/webinars` | webinars | Typed |
| POST | `/users/{userId}/webinars` | webinarCreate | Typed |
| DELETE | `/webinars/{webinarId}` | webinarDelete | Typed |
| GET | `/webinars/{webinarId}` | webinar | Typed |
| PATCH | `/webinars/{webinarId}` | webinarUpdate | Typed |
| POST | `/webinars/{webinarId}/batch_registrants` | addBatchWebinarRegistrants | Typed |
| GET | `/webinars/{webinarId}/branding` | getWebinarBranding | Typed |
| DELETE | `/webinars/{webinarId}/branding/name_tags` | deleteWebinarBrandingNameTag | Typed |
| POST | `/webinars/{webinarId}/branding/name_tags` | createWebinarBrandingNameTag | Typed |
| PATCH | `/webinars/{webinarId}/branding/name_tags/{nameTagId}` | updateWebinarBrandingNameTag | Typed |
| DELETE | `/webinars/{webinarId}/branding/virtual_backgrounds` | deleteWebinarBrandingVB | Typed |
| PATCH | `/webinars/{webinarId}/branding/virtual_backgrounds` | setWebinarBrandingVB | Typed |
| POST | `/webinars/{webinarId}/branding/virtual_backgrounds` | uploadWebinarBrandingVB | Typed |
| DELETE | `/webinars/{webinarId}/branding/wallpaper` | deleteWebinarBrandingWallpaper | Typed |
| POST | `/webinars/{webinarId}/branding/wallpaper` | uploadWebinarBrandingWallpaper | Typed |
| POST | `/webinars/{webinarId}/invite_links` | webinarInviteLinksCreate | Typed |
| GET | `/webinars/{webinarId}/jointoken/live_streaming` | webinarLiveStreamingJoinToken | Typed |
| GET | `/webinars/{webinarId}/jointoken/local_archiving` | webinarLocalArchivingArchiveToken | Typed |
| GET | `/webinars/{webinarId}/jointoken/local_recording` | webinarLocalRecordingJoinToken | Typed |
| GET | `/webinars/{webinarId}/livestream` | getWebinarLiveStreamDetails | Typed |
| PATCH | `/webinars/{webinarId}/livestream` | webinarLiveStreamUpdate | Typed |
| PATCH | `/webinars/{webinarId}/livestream/status` | webinarLiveStreamStatusUpdate | Typed |
| DELETE | `/webinars/{webinarId}/panelists` | webinarPanelistsDelete | Typed |
| GET | `/webinars/{webinarId}/panelists` | webinarPanelists | Typed |
| POST | `/webinars/{webinarId}/panelists` | webinarPanelistCreate | Typed |
| DELETE | `/webinars/{webinarId}/panelists/{panelistId}` | webinarPanelistDelete | Typed |
| GET | `/webinars/{webinarId}/polls` | webinarPolls | Typed |
| POST | `/webinars/{webinarId}/polls` | webinarPollCreate | Typed |
| DELETE | `/webinars/{webinarId}/polls/{pollId}` | webinarPollDelete | Typed |
| GET | `/webinars/{webinarId}/polls/{pollId}` | webinarPollGet | Typed |
| PUT | `/webinars/{webinarId}/polls/{pollId}` | webinarPollUpdate | Typed |
| GET | `/webinars/{webinarId}/registrants` | webinarRegistrants | Typed |
| POST | `/webinars/{webinarId}/registrants` | webinarRegistrantCreate | Typed |
| GET | `/webinars/{webinarId}/registrants/questions` | webinarRegistrantsQuestionsGet | Typed |
| PATCH | `/webinars/{webinarId}/registrants/questions` | webinarRegistrantQuestionUpdate | Typed |
| PUT | `/webinars/{webinarId}/registrants/status` | webinarRegistrantStatus | Typed |
| DELETE | `/webinars/{webinarId}/registrants/{registrantId}` | deleteWebinarRegistrant | Typed |
| GET | `/webinars/{webinarId}/registrants/{registrantId}` | webinarRegistrantGet | Typed |
| POST | `/webinars/{webinarId}/sip_dialing` | getWebinarSipDialingWithPasscode | Typed |
| PUT | `/webinars/{webinarId}/status` | webinarStatus | Typed |
| DELETE | `/webinars/{webinarId}/survey` | webinarSurveyDelete | Typed |
| GET | `/webinars/{webinarId}/survey` | webinarSurveyGet | Typed |
| PATCH | `/webinars/{webinarId}/survey` | webinarSurveyUpdate | Typed |
| GET | `/webinars/{webinarId}/token` | webinarToken | Typed |
| GET | `/webinars/{webinarId}/tracking_sources` | getTrackingSources | Typed |