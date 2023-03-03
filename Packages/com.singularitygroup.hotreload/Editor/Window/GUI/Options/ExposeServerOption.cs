﻿using System;
using System.Threading.Tasks;
using SingularityGroup.HotReload.Editor.Cli;
using UnityEditor;
using UnityEngine;

namespace SingularityGroup.HotReload.Editor {
    internal sealed class ExposeServerOption : ComputerOptionBase {

        public override string ShortSummary => "Allow Mobile Builds to Connect";
        public override string Summary => "Allow Mobile Builds to Connect (WiFi)";

        private readonly string dataPath;

        public ExposeServerOption() {
            // get dataPath on main thread.
            dataPath = Application.dataPath;
        }

        public override void InnerOnGUI() {
            string description;
            if (GetValue()) {
                description = "The HotReload server is reachable from devices on the same Wifi network";
            } else {
                description = "The HotReload server is available to your computer only. Other devices cannot connect to it.";
            }
            EditorGUILayout.LabelField(description, HotReloadWindowStyles.WrapStyle);
        }

        public override bool GetValue() {
            return HotReloadPrefs.ExposeServerToLocalNetwork;
        }

        public override void SetValue(SerializedObject so, bool val) {
            // AllowAndroidAppToMakeHttpRequestsOption
            if (val == HotReloadPrefs.ExposeServerToLocalNetwork) {
                return;
            }

            HotReloadPrefs.ExposeServerToLocalNetwork = val;
            if (val) {
                // they allowed this one for mobile builds, so now we allow everything else needed for player build to work with HR
                new AllowAndroidAppToMakeHttpRequestsOption().SetValue(so, true);
            }
            RunTask(() => {
                RunOnMainThreadSync(() => {
                    var isRunningResult = ServerHealthCheck.I.IsServerHealthy;
                    if (isRunningResult) {
                        var restartServer = EditorUtility.DisplayDialog("Hot Reload",
                            $"When changing '{Summary}', the Hot Reload server must be restarted for this to take effect." +
                            "\nDo you want to restart it now?",
                            "Restart server", "Don't restart");
                        if (restartServer) {
                            bool exposeServerToNetwork = HotReloadPrefs.ExposeServerToLocalNetwork;
                            CodePatcher.I.ClearPatchedMethods();
                            RunTask(() => HotReloadCli.RestartAsync(dataPath, exposeServerToNetwork));
                        }
                    }
                });
            });
        }

        void RunTask(Action action) {
            var token = HotReloadWindow.Current.cancelToken;
            Task.Run(() => {
                if (token.IsCancellationRequested) return;
                try {
                    action();
                } catch (Exception ex) {
                    ThreadUtility.LogException(ex, token);
                }
            }, token);
        }
        
        void RunTask(Func<Task> action) {
            var token = HotReloadWindow.Current.cancelToken;
            Task.Run(async () => {
                if (token.IsCancellationRequested) return;
                try {
                    await action();
                } catch (Exception ex) {
                    ThreadUtility.LogException(ex, token);
                }
            }, token);
        }

        void RunOnMainThreadSync(Action action) {
            ThreadUtility.RunOnMainThread(action, HotReloadWindow.Current.cancelToken);
        }
    }
}
