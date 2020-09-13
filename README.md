# UnityAssetComment
サーバ介して同一プロジェクト内で同期するアセットコメント機能<br>
 <br>
コメントは 
- プレハブ(バリアントは別プレハブ扱い)
- シーン上の未プレハブオブジェト(シーンファイル作成済みの場合のみ)

の単位で同期されます。

## Demo
![screenCapture](https://user-images.githubusercontent.com/17733911/93001462-0f48f700-f56a-11ea-9bed-c5f40109c8b1.gif)

## Setting
[Firebase](https://console.firebase.google.com/)でプロジェクトを作成し、<br>
`AssetComment/Editor/FirebaseConfig.cs`にRealtime DatabaseのURLとシークレットを設定します。<br>
<br>
シークレットは<br>
https://console.firebase.google.com/project/プロジェクトID/settings/serviceaccounts/databasesecrets<br>
確認できます。

## How To Use
`Comment/Create Window`でコメント専用のウィンドウが開きます。<br>
`Comment/Show SceneView`でシーンビュー上でのコメント窓の表示・非表示が設定できます。(選択中オブジェクトのみ表示)

## License
[MIT](https://github.com/Itoen/UnityAssetComment/blob/master/LICENSE)
