import QtQuick 2.7
import QtQuick.Controls 2.0
import QtQuick.Layouts 1.0
import Qt.labs.platform 1.1
import app 1.0


Item {
    TrayModel {
        id: model
        Component.onCompleted: function() {
            model.onLoad();
        }
    }
    
    SystemTrayIcon {
        id: trayIcon
        visible: true
        icon.source: "https://img.icons8.com/android/24/000000/music.png"

        menu: Menu {
            MenuItem {
                text: qsTr("Quit")
                onTriggered: Qt.quit()
            }
        }
    }
}