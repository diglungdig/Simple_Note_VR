# Simple Note VR
> A simple notification system to display plain text in VR for Unity engine(that looks like Tilt Brush's way of doing it :D)

![Simple Note VR](https://github.com/diglungdig/SimpleNoteVR/blob/master/Screenshots/readmeGif.gif)

## How to use

Please go to the release page to download the package.

Import the package into your own proejct, simply drag the "NotificationVR" prefab to the scene.

Then, in your own script, call "SimpleNoteVR.Instance.Notfiy(string words, float lingertime)" to trigger plain text notifications.([lingertime is the duration that your notifcation will last for])

You can also choose to call "SimpleNoteVR.Instance.Notfiy_Hold(string words)". This function will instead keep the notification in place until you call "SimpleNoteVR.Instance.Notfiy_Release" to release it.
Notify_Hold/Notify_Release functions might be useful in cases like notifying player a loading process is ongoing/finished. 

Features:

1. UI overlay shaders making sure the notifications always stay on top of everything
2. Changable text and background color
3. Changable text font
4. Changable sprite background 

## License

This project is under a MIT license which allows for third party modification and expansion.

