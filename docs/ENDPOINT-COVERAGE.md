Total operations: 186
Typed in v1: 140
Raw-only (still fully callable via CallAsync): 46

| Domain | Total | Typed v1 | Raw-only |
|---|---|---|---|
| Devices/H.323 | 17 | 0 | 17 |
| Live Meeting Controls | 4 | 0 | 4 |
| Meeting Summaries | 4 | 3 | 1 |
| Meetings core | 27 | 27 | 0 |
| Polls | 7 | 5 | 2 |
| Recordings/Archiving | 23 | 23 | 0 |
| Registrants | 8 | 5 | 3 |
| Reports | 24 | 24 | 0 |
| SIP Phones | 4 | 0 | 4 |
| TSP | 8 | 0 | 8 |
| Templates | 2 | 0 | 2 |
| Tracking Fields | 5 | 0 | 5 |
| Webinars | 53 | 53 | 0 |


### Devices/H.323
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/devices` | listDevices | Raw |
| POST | `/devices` | addDevice | Raw |
| GET | `/devices/groups` | Getzdmgroupinfo | Raw |
| POST | `/devices/zpa/assignment` | Assigndevicetoauser/commonarea | Raw |
| GET | `/devices/zpa/settings` | GetZpaDeviceListProfileSettingOfaUser | Raw |
| POST | `/devices/zpa/upgrade` | UpgradeZpas/app | Raw |
| DELETE | `/devices/zpa/vendors/{vendor}/mac_addresses/{macAddress}` | DeleteZpaDeviceByVendorAndMacAddress | Raw |
| GET | `/devices/zpa/zdm_groups/{zdmGroupId}/versions` | GetZpaVersioninfo | Raw |
| DELETE | `/devices/{deviceId}` | deleteDevice | Raw |
| GET | `/devices/{deviceId}` | getDevice | Raw |
| PATCH | `/devices/{deviceId}` | updateDevice | Raw |
| PATCH | `/devices/{deviceId}/assign_group` | assginGroup | Raw |
| PATCH | `/devices/{deviceId}/assignment` | changeDeviceAssociation | Raw |
| GET | `/h323/devices` | deviceList | Raw |
| POST | `/h323/devices` | deviceCreate | Raw |
| DELETE | `/h323/devices/{deviceId}` | deviceDelete | Raw |
| PATCH | `/h323/devices/{deviceId}` | deviceUpdate | Raw |

### Live Meeting Controls
| Method | Path | operationId | v1 status |
|---|---|---|---|
| DELETE | `/live_meetings/{meetingId}/chat/messages/{messageId}` | deleteMeetingChatMessageById | Raw |
| PATCH | `/live_meetings/{meetingId}/chat/messages/{messageId}` | updateMeetingChatMessageById | Raw |
| PATCH | `/live_meetings/{meetingId}/events` | inMeetingControl | Raw |
| PATCH | `/live_meetings/{meetingId}/rtms_app/status` | meetingRTMSStatusUpdate | Raw |

### Meeting Summaries
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/meetings/meeting_summaries` | Listmeetingsummaries | Typed |
| DELETE | `/meetings/{meetingId}/meeting_summary` | Deletemeetingorwebinarsummary | Typed |
| GET | `/meetings/{meetingId}/meeting_summary` | Getameetingsummary | Typed |
| GET | `/users/{userId}/meeting_summaries` | ListUserMeetingSummaries | Raw |

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
| POST | `/meetings/{meetingId}/batch_polls` | createBatchPolls | Raw |
| GET | `/meetings/{meetingId}/polls` | meetingPolls | Typed |
| POST | `/meetings/{meetingId}/polls` | meetingPollCreate | Typed |
| DELETE | `/meetings/{meetingId}/polls/{pollId}` | meetingPollDelete | Typed |
| GET | `/meetings/{meetingId}/polls/{pollId}` | meetingPollGet | Typed |
| PUT | `/meetings/{meetingId}/polls/{pollId}` | meetingPollUpdate | Typed |
| GET | `/past_meetings/{meetingId}/polls` | listPastMeetingPolls | Raw |

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
| POST | `/meetings/{meetingId}/batch_registrants` | addBatchRegistrants | Raw |
| GET | `/meetings/{meetingId}/registrants` | meetingRegistrants | Typed |
| POST | `/meetings/{meetingId}/registrants` | meetingRegistrantCreate | Typed |
| GET | `/meetings/{meetingId}/registrants/questions` | meetingRegistrantsQuestionsGet | Raw |
| PATCH | `/meetings/{meetingId}/registrants/questions` | meetingRegistrantQuestionUpdate | Raw |
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
| GET | `/sip_phones/phones` | ListSIPPhonePhones | Raw |
| POST | `/sip_phones/phones` | EnableSIPPhonePhones | Raw |
| DELETE | `/sip_phones/phones/{phoneId}` | deleteSIPPhonePhones | Raw |
| PATCH | `/sip_phones/phones/{phoneId}` | UpdateSIPPhonePhones | Raw |

### TSP
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/tsp` | tsp | Raw |
| PATCH | `/tsp` | tspUpdate | Raw |
| GET | `/users/{userId}/tsp` | userTSPs | Raw |
| POST | `/users/{userId}/tsp` | userTSPCreate | Raw |
| PATCH | `/users/{userId}/tsp/settings` | tspUrlUpdate | Raw |
| DELETE | `/users/{userId}/tsp/{tspId}` | userTSPDelete | Raw |
| GET | `/users/{userId}/tsp/{tspId}` | userTSP | Raw |
| PATCH | `/users/{userId}/tsp/{tspId}` | userTSPUpdate | Raw |

### Templates
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/users/{userId}/meeting_templates` | listMeetingTemplates | Raw |
| POST | `/users/{userId}/meeting_templates` | meetingTemplateCreate | Raw |

### Tracking Fields
| Method | Path | operationId | v1 status |
|---|---|---|---|
| GET | `/tracking_fields` | trackingfieldList | Raw |
| POST | `/tracking_fields` | trackingfieldCreate | Raw |
| DELETE | `/tracking_fields/{fieldId}` | trackingfieldDelete | Raw |
| GET | `/tracking_fields/{fieldId}` | trackingfieldGet | Raw |
| PATCH | `/tracking_fields/{fieldId}` | trackingfieldUpdate | Raw |

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