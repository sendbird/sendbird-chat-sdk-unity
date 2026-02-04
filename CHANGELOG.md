# Change Log

## 4.1.3 (Feb 4, 2026)
### Bug Fixes
- Fixed WebGL memory issue by optimizing JSON parsing for message deserialization

## 4.1.2 (Nov 3, 2025)
### Improvements
- Removed unnecessary URL encoding from SB-SDK-User-Agent header

## 4.1.1 (Oct 28, 2025)
### Bug Fixes
- Resolved WebSocket connection failure due to User-Agent header on .NET Framework

## 4.1.0 (Nov 29, 2024)
### Features
- Added `SetPushTriggerOption` to `SendbirdChatClient`
- Added `GetPushTriggerOption` to `SendbirdChatClient`
- Added `SetMyPushTriggerOption` to `SbGroupChannel`
- Added `GetMyPushTriggerOption` to `SbGroupChannel`
- Added `SbPushTriggerOption`
### Bug Fixes
- Fixed an issue with `SendbirdChat.BlockUser` where 'User not found error' occurs due to URL encoding

## 4.1.0 (Nov 29, 2024)
### Features
- Added `SetPushTriggerOption` to `SendbirdChatClient`
- Added `GetPushTriggerOption` to `SendbirdChatClient`
- Added `SetMyPushTriggerOption` to `SbGroupChannel`
- Added `GetMyPushTriggerOption` to `SbGroupChannel`
- Added `SbPushTriggerOption`
### Bug Fixes
- Fixed an issue with `SendbirdChat.BlockUser` where 'User not found error' occurs due to URL encoding

## 4.0.1 (Nov 4, 2024)
### Bug Fixes
- Fixed an issue where build failed on the Windows platform

## 4.0.0 (Sep 25, 2024)
### Features
- Added support for WebGL

## 4.0.0-beta.3 (Apr 9, 2024)
### Improvements
- Added SendbirdChatPrivacyInfo.xcprivacy for Apple Privacy Manifest

## 4.0.0-beta.2 (Mar 7, 2024)
### Improvements
- Changed the JSON library from a binary to a dependency form
- Added support for Unity2019.4
- Added support for .NET 4.x

## 4.0.0-beta.1 (Sep 15, 2023)
### Bug Fixes
 - Fixed the bug regarding the URL encoding

## 4.0.0-beta (Aug 25, 2023)
### Features
 - Group channel collection
 - Message collection
 - Pinned message
 - Message threading
 - Adding extra data to a message
 - Multiple App ID support

