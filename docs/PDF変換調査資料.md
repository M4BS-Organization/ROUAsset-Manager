# PDF to Markdown 変換ツール調査レポート

調査日: 2026-03-10
対象: 100ページ程度・約1.2MBのPDF（日本語コンテンツを含む可能性あり）の高精度Markdown変換

---

## 概要

本レポートは、大規模PDFをMarkdownに変換するための主要ツール・アプローチを比較する。
評価軸は以下の通り：

- 変換精度（表・数式・画像・見出し・リスト）
- 日本語テキストの対応状況
- 100ページ以上の処理能力
- Windows 11 でのセットアップ容易性
- 無料・オープンソース可否
- 速度・パフォーマンス
- 制約・注意点

---

## 1. marker（VikParuchuri / datalab-to）

**リポジトリ:** https://github.com/datalab-to/marker
**種別:** オープンソース（ただし商用利用にはライセンス制限あり）

### 概要

深層学習ベースのPDF→Markdown変換ツール。Surya OCRエンジンをデフォルトで使用し、PDFのほかPPTX・DOCX・XLSX・HTML・EPUBにも対応する。オプションで LLM（GPT-4 等）と組み合わせることで精度をさらに向上させることができる（`--use_llm` フラグ）。

### 精度

| 要素 | 評価 | 備考 |
|------|------|------|
| 表 | 良好〜優秀 | `--use_llm` 時はページをまたぐ表もマージ可 |
| 数式（インライン・ブロック） | 良好 | `--use_llm` でインライン数式も整形 |
| 見出し・リスト | 優秀 | ヘッダー/フッターの自動除去あり |
| 画像 | 対応あり | 抽出は可能だが埋め込み画像の説明は限定的 |
| コードブロック | 良好 | 言語指定は保持されない場合あり |

Llamaparse・Mathpix等のクラウドサービスより有利なベンチマーク結果を公表。

### 日本語対応

**対応。** OCRエンジンの Surya は90以上の言語をサポートしており、日本語（`-l ja`）を明示的にサポートしている。ただし初期バージョン（marker-pdf 0.1.x 系）には「CJK文字セットは未対応」という記述があったが、現行バージョン（datalab-to/marker）では Surya の多言語対応により改善されている。重要なドキュメントでは実際にテストして確認することを推奨。

### 100ページ対応

対応。バッチモードではH100 GPU上で約25ページ/秒のスループット。CPUのみでも動作するが大幅に遅くなる。

### Windows 11 セットアップ

- `pip install marker-pdf` または `pip install marker-pdf[full]`
- Python 3.10+ 必須
- PyTorchが必要（GPUを使う場合はCUDA + NVIDIA GPU 8GB VRAM以上を推奨）
- CPUのみでも動作するが低速
- conda環境での管理を推奨

### 費用

オープンソース（GPLコード + AIモデルは修正AI Pubsライセンス）。商用利用時はライセンス確認が必要。LLMモードを使う場合はAPI費用が別途発生。

### 速度

- GPU（H100）: 約25ページ/秒（バッチモード）
- CPU（一般的なPC）: 数ページ/分程度（推定）
- 100ページPDFはGPUあり環境で数分以内に完了の見込み

### 制約

- 商用利用のライセンスに注意
- OCR精度は言語・フォントによって変動
- GPU推奨（CPU時は遅い）
- LLMモード利用時の追加コスト・遅延

---

## 2. pymupdf4llm（PyMuPDF）

**URL:** https://pymupdf.readthedocs.io/en/latest/pymupdf4llm/
**PyPI:** https://pypi.org/project/pymupdf4llm/
**種別:** オープンソース（AGPL / 商用ライセンスあり）

### 概要

MuPDF/PyMuPDFをベースとした、LLM・RAG向けのMarkdown抽出ライブラリ。OCRにはTesseractまたはRapidOCRを使用。GitHub互換Markdownを出力し、フォントサイズから見出しを検出する。

### 精度

| 要素 | 評価 | 備考 |
|------|------|------|
| テキスト | 優秀 | ベクターPDFは非常に高速・高精度 |
| 表 | やや弱い | 縦罫線のない単純な表は正確に変換できないケースあり |
| 数式 | 基本的なみ | 複雑な数式は非対応 |
| 見出し | 良好 | フォントサイズ基準で`#`タグを付与 |
| コードブロック | 対応（言語指定なし） | |
| 画像 | 限定的 | テキスト抽出が主目的 |

表の処理が弱い点はよく指摘される。PyMuPDF Layout（有料Pro版）を組み合わせるとAIベースのレイアウト解析で大幅改善。

### 日本語対応

**条件付きで対応。** ベクターPDF（テキストが埋め込まれている）であれば、PyMuPDFは日本語を含む多言語テキストをそのまま抽出できる。スキャンPDF（画像PDF）の場合はOCR（Tesseract日本語モデルまたはRapidOCR）が必要。Tesseractの日本語パッケージ（`jpn`）を別途インストールすれば対応可能。

### 100ページ対応

対応。10ページ（長い表あり）を約0.12秒で処理できるほど非常に高速。100ページPDFも数秒〜十数秒程度で処理完了の見込み。

### Windows 11 セットアップ

- `pip install pymupdf4llm`（PyMuPDFも自動インストール）
- 最も簡単なセットアップの一つ
- スキャンPDFに対応するにはTesseractを別途インストール（Windows版バイナリあり）
- GPU不要

### 費用

オープンソース（AGPLまたは商用ライセンス）。基本無料。

### 速度

最速クラス。0.12秒/10ページ（M1 Mac実測値）。Windowsでも非常に高速。

### 制約

- 表の精度が低い（縦罫線なしの場合）
- スキャンPDFの精度はTesseract/RapidOCRに依存
- 複雑な数式・化学式には非対応
- レイアウト解析の改善にはPro版（有料）が必要

---

## 3. docling（IBM）

**GitHub:** https://github.com/docling-project/docling
**公式サイト:** https://docling-project.github.io/docling/
**種別:** オープンソース（MIT）

### 概要

IBMが開発したドキュメント変換ツール。PDF・画像・DOCX・HTMLなどに対応し、構造化されたMarkdown/JSON出力を生成。AIモデル（Granite-Docling等）を組み合わせて高精度なレイアウト認識・表抽出を実現。

### 精度

| 要素 | 評価 | 備考 |
|------|------|------|
| 表 | 優秀 | 関連情報をすべて抽出。最も構造的に整然とした出力 |
| レイアウト認識 | 優秀 | F1スコア0.86（Granite-Docling比較） |
| 見出し・リスト | 優秀 | 構造情報が豊富 |
| 数式 | 良好 | |
| OCR精度 | 良好 | Full Page OCR F1スコア0.84 |

企業向けRAGパイプラインで高い評価を得ており、構造的な出力品質はトップクラス。

### 日本語対応

**実験的に対応。** Granite-Doclingモデルが日本語・中国語・アラビア語を実験的にサポート。ただし英語がプライマリターゲットであり、日本語は「早期サポート（early-stage）」と位置づけられている。本番環境での日本語精度は不確実であり、テストが必要。

### 100ページ対応

対応。ただしmarker/pymupdf4llmと比較して処理オーバーヘッドが大きい。

### Windows 11 セットアップ

- `pip install docling`
- Python 3.10+ 必須
- AIモデルのダウンロードが必要（初回起動時）
- GPU推奨だがCPUでも動作

### 費用

MIT ライセンスで完全無料・商用利用可。

### 速度

markerより遅め。大きなモデルを使用するため処理時間は長い。

### 制約

- 日本語サポートは実験的（Experimental）
- 処理速度が遅い
- モデルのダウンロード（初回）が必要
- メモリ使用量が大きい

---

## 4. Mathpix

**公式サイト:** https://mathpix.com/
**日本語OCR記事:** https://mathpix.com/blog/japanese-ocr
**種別:** 商用SaaS（APIあり）

### 概要

数式・科学文書に特化した高精度OCRサービス。PDF→LaTeX/Markdown/DOCX変換に対応。28言語のOCRをサポートし、日本語対応を明示的に謳っている。

### 精度

| 要素 | 評価 | 備考 |
|------|------|------|
| 数式 | 最高クラス | PhD級の数学・物理・統計に対応 |
| 表（数式含む） | 優秀 | 数学記号・完全な数式を含む表に強い |
| 日本語テキスト | 優秀 | 「日本語認識の最良ツール」と自称 |
| 化学構造式 | 対応 | |
| 一般テキスト | 優秀 | |

### 日本語対応

**優秀。** Mathpixは日本語OCRを明示的にサポート。「数式を含む日本語認識の最良ツール」と公式に位置づけており、日本語PDFのDOCX・Markdown・LaTeX変換が可能。

### 100ページ対応

対応。ただしAPIを使ったバッチ処理が必要。100ページ = $0.50（2025年3月時点の価格：$0.005/ページ）。

### Windows 11 セットアップ

APIベースのため、インストール不要。Python等でHTTPリクエストを送るだけ。セットアップ難易度は最低。

### 費用

**有料。** APIは$0.005/ページ（2025年3月現在）。
- 月20ページまで無料
- Pro プラン：月1,000ページ含む（サブスクリプション料別途）
- 100ページ処理 = 約$0.50

### 速度

クラウドAPIのため処理はほぼリアルタイム〜数分。

### 制約

- 有料（ただし100ページ程度なら低コスト）
- インターネット接続必須
- PDFをサードパーティサーバーに送信（機密文書には注意）
- 無料枠は月20ページと少ない

---

## 5. Claude API（PDFサポート）

**公式ドキュメント:** https://platform.claude.com/docs/en/build-with-claude/pdf-support
**種別:** 商用SaaS（Anthropic API）

### 概要

Anthropic の Claude API はPDFを直接入力として受け付け、各ページを画像＋テキストの組み合わせで処理する。Markdownへの変換は「PDFを読んでMarkdown形式で出力せよ」というプロンプトを与えることで実現する。視覚的理解（チャート・ダイアグラム・図表）にも対応。

### 精度

| 要素 | 評価 | 備考 |
|------|------|------|
| テキスト全般 | 優秀 | ビジョン＋テキスト抽出の組み合わせ |
| 表 | 良好〜優秀 | レイアウトを視覚的に理解して変換 |
| 画像・チャート | 優秀 | 他のツールで困難な視覚要素も解釈可 |
| 数式 | 良好 | LaTeX形式での出力指定も可能 |
| 日本語テキスト | 優秀 | Claudeは日本語を高精度で処理 |

### 日本語対応

**優秀。** Claudeは日本語のネイティブサポートを持つ大規模言語モデルであり、日本語テキストの認識・構造化・Markdown変換において最高レベルの品質が期待できる。

### 100ページ対応

**1リクエストあたり最大100ページ・32MBまで。**
1.2MBのPDFは100ページ以内であれば1回のAPIコールで処理可能。ただし、100ページPDFは大量のトークンを消費する（1,500〜3,000トークン/ページ × 画像コスト）。

| 制限 | 値 |
|------|-----|
| 最大リクエストサイズ | 32MB |
| 最大ページ数/リクエスト | 100ページ |
| コスト概算 | 1ページ約1,500〜3,000トークン + 画像コスト |

### Windows 11 セットアップ

- `pip install anthropic`
- APIキーを取得するだけ
- セットアップ難易度は最低レベル

### 費用

**有料。** トークン使用量に応じた従量課金。
- claude-sonnet-4-6（推奨）：入力 $3/MTok
- 100ページPDF（各ページ2,000トークン＋画像コスト）：推定$1〜$3程度

### 速度

APIレイテンシに依存。100ページのバッチ処理には数分〜10分程度を見込む。

### 制約

- 有料（100ページ処理で$1〜$3程度）
- インターネット接続必須・機密文書の取り扱いに注意
- 100ページを超えるPDFは分割が必要
- Markdown変換の「指示」がプロンプト依存（出力品質をプロンプトでコントロール必要）
- 大量処理には Batch API の利用が推奨（コスト50%削減）

---

## 6. pandoc

**公式サイト:** https://pandoc.org/
**種別:** オープンソース（GPL）

### 概要

汎用ドキュメント変換ツール。Markdownを含む多数のフォーマット間変換に対応。ただし、PDF→Markdownの方向はネイティブでは非対応。通常は他ツール（pdftotext等）でテキスト抽出後にpandocでMarkdown整形というパイプラインになる。

### 精度

| 要素 | 評価 | 備考 |
|------|------|------|
| 基本テキスト | 普通 | 単純なPDFには使えるが精度に限界 |
| 表 | 弱い | 複雑な表はpandocのモデルに収まらないことが多い |
| 数式 | 限定的 | |
| レイアウト | 弱い | マージン等の書式情報は失われる |

オープンソースベンチマークでは、高度に構造化された文書での精度は60〜70%程度との報告あり。

### 日本語対応

pdftotext等のテキスト抽出ツールが日本語を正しく処理できれば、pandocによる整形は問題ない。ただしスキャンPDFには対応不可。

### 100ページ対応

技術的には可能だが、PDF直接変換の品質に問題がある。

### Windows 11 セットアップ

- Windowsインストーラーあり（msi）
- 追加で Ghostscript・pdftotext（poppler）等が必要
- セットアップはやや複雑

### 費用

完全無料。

### 速度

高速（テキスト変換ベース）。

### 制約

- **PDF→Markdownのネイティブ変換は非対応**（迂回路が必要）
- 表・レイアウトの精度が低い
- スキャンPDFには全く対応できない
- 他のツールと比較して明らかに品質が劣る

---

## 7. nougat（Meta）

**GitHub:** https://github.com/facebookresearch/nougat
**Hugging Face:** https://huggingface.co/facebook/nougat-base
**種別:** オープンソース（MIT）

### 概要

MetaのAI研究チームが開発した、学術論文PDF→Markdown変換に特化したニューラルOCRモデル。Transformerベースで、arXiv・PMCの論文データで学習されている。数式の認識に強みを持つ。

### 精度

| 要素 | 評価 | 備考 |
|------|------|------|
| 数式（学術論文） | 最高クラス | 学術論文に特化 |
| 表（学術論文） | 良好 | |
| 英語テキスト | 優秀 | |
| 非英語テキスト | **不可** | |

### 日本語対応

**非対応。** 公式に「英語の論文に特化しており、中国語・ロシア語・日本語等は動作しない」と明記されている。日本語文書への使用は不適切。

### 100ページ対応

技術的には可能だが、処理時間が長い（GPUを要する）。

### Windows 11 セットアップ

- `pip install nougat-ocr`
- PyTorch + CUDA環境が必要
- GPU（VRAM 8GB+）推奨

### 費用

無料（オープンソース）。

### 速度

GPUを必要とし、比較的低速。

### 制約

- **日本語不対応（根本的な制約）**
- 英語学術論文専用
- GPU必須
- 汎用文書への転用は困難

---

## 8. pdf2md系ツール（npm）

**代表的なもの：**
- `pdf2md` (npm): https://www.npmjs.com/package/pdf2md
- `@opendocsg/pdf2md` (npm): https://www.npmjs.com/package/@opendocsg/pdf2md
- `pdf2md-js` (npm): https://www.npmjs.com/package/pdf2md-js

### 概要

Node.js/JavaScriptベースのPDF→Markdown変換ツール群。pdf.jsやpdfminer相当の技術でテキスト抽出を行うものが多い。`pdf2md-js`はOpenAI・Claude・Gemini等のビジョンモデルと統合したより高精度なバリアント。

### 精度

| 要素 | 評価 | 備考 |
|------|------|------|
| 基本テキスト | 普通 | シンプルPDFなら実用的 |
| 表 | 弱い〜普通 | ビジョンモデル統合版は改善 |
| 数式 | 弱い | |
| 複雑レイアウト | 弱い | |

### 日本語対応

pdf2md-jsはOpenAI/Claude等のビジョンモデルを使うため、モデルが日本語を処理できれば対応可。従来型のテキスト抽出ベースのpdf2mdでは、フォントのエンコーディングによっては日本語が文字化けすることがある。

### 100ページ対応

可能だが、ツールによっては大きなPDFで不安定になる場合がある。

### Windows 11 セットアップ

- `npm install -g pdf2md` または同等
- Node.js環境があれば簡単
- ビジョンモデル統合版はAPIキーが必要

### 費用

基本は無料。ビジョンモデルAPIを使う場合は従量課金。

### 速度

ビジョンモデルを使わないものは高速。APIコールが必要なものはレイテンシあり。

### 制約

- 複雑なPDFの変換品質が低い
- ビジョンモデル統合版はAPIコスト発生
- 成熟度・メンテナンス状況がツールによって大きく異なる

---

## 9. 補足：MinerU（opendatalab）

本調査の調査過程で、比較対象として特筆すべきツールが発見されたため追記する。

**GitHub:** https://github.com/opendatalab/MinerU
**種別:** オープンソース（AGPL）

### 概要

中国・上海AIラボが開発した高品質なPDF→Markdown変換ツール。PPOCRv5を使用し、109言語に対応。日本語・繁体字・CJK文字を含む15,000文字以上を認識。学術論文・複雑なレポートに適しており、商用ツールに近い品質を実現している。

### 日本語対応

**優秀。** PP-OCRv4_server_rec_docモデルが日本語および繁体字に対応しており、特殊文字含め15,000文字以上を認識。v2.6.x系での大幅な精度向上も報告されている。

### Windows 11 セットアップ

公式にWindows対応。v2.6.5でWindowsネイティブアクセラレーションが追加された。

### 費用

オープンソース・無料（商用利用時はAGPLに注意）。

---

## 総合比較表

| ツール | 変換精度（全般） | 日本語対応 | 100ページ | Windows設定難易度 | 費用 | 速度 | 推奨度 |
|--------|:--------------:|:--------:|:--------:|:----------------:|:----:|:----:|:------:|
| **marker** | ★★★★☆ | ★★★★☆ | ✓ | 中（PyTorch必要） | 無料（商用注意） | ★★★★☆ | A |
| **pymupdf4llm** | ★★★☆☆ | ★★★☆☆ | ✓ | 易 | 無料 | ★★★★★ | B |
| **docling** | ★★★★★ | ★★★☆☆ | ✓ | 中 | 無料（MIT） | ★★★☆☆ | B+ |
| **Mathpix** | ★★★★★ | ★★★★★ | ✓（API） | 易（API） | 有料（$0.005/p） | ★★★★☆ | A |
| **Claude API** | ★★★★★ | ★★★★★ | ✓（100p制限） | 易（API） | 有料（$1〜3/100p） | ★★★☆☆ | A |
| **pandoc** | ★★☆☆☆ | ★★★☆☆ | ✓ | 中 | 無料 | ★★★★★ | D |
| **nougat** | ★★★★☆ | **✗不可** | ✓ | 難（GPU必要） | 無料 | ★★☆☆☆ | D |
| **pdf2md系** | ★★☆☆☆ | ★★★☆☆ | 条件付 | 易 | 無料〜有料 | ★★★☆☆ | C |
| **MinerU** | ★★★★★ | ★★★★☆ | ✓ | 中 | 無料（AGPL注意） | ★★★★☆ | A |

---

## 推奨ランキング（本ユースケース向け：100ページ・日本語コンテンツ）

### 第1位: **marker**（ローカル・高精度・日本語対応）

最も汎用性が高く、日本語対応（Surya経由）・高精度・ローカル処理という三拍子がそろっている。GPUが利用可能な環境であれば最有力候補。`--use_llm`フラグで精度をさらに向上させることができる。

**推奨シナリオ:** GPU環境が利用可能、またはCPUでも時間をかけて処理できる場合。機密文書のためローカル処理が必要な場合。

```bash
pip install marker-pdf
marker_single input.pdf --output_dir ./output --langs ja,en
```

### 第2位: **Claude API**（最高精度・最高日本語品質・即時利用可）

日本語テキストの認識・構造化品質は最高レベル。セットアップが最も簡単で、100ページPDFを1リクエストで処理できる。視覚的なレイアウト理解にも優れる。コストは100ページで$1〜$3程度。

**推奨シナリオ:** 最高品質が求められる場合、機密性が問題ない場合、開発速度を優先する場合。

```python
import anthropic, base64
client = anthropic.Anthropic()
with open("input.pdf", "rb") as f:
    pdf_data = base64.b64encode(f.read()).decode()
response = client.messages.create(
    model="claude-sonnet-4-6",
    max_tokens=8192,
    messages=[{
        "role": "user",
        "content": [
            {"type": "document", "source": {"type": "base64", "media_type": "application/pdf", "data": pdf_data}},
            {"type": "text", "text": "このPDFの内容をMarkdown形式に変換してください。表・見出し・リストの構造を正確に再現してください。"}
        ]
    }]
)
```

### 第3位: **Mathpix**（数式・日本語の両立が必要な場合）

数式を多く含む日本語技術文書では最高の選択肢。日本語OCRの品質を明示的に保証している数少ないサービス。コストも100ページ$0.50と低い。

**推奨シナリオ:** 数式・化学式・日本語が混在するPDF。学術論文・技術仕様書。

### 第4位: **MinerU**（完全オープンソース・高品質・日本語対応）

商用利用可能なAGPLライセンスで、日本語を含む多言語に高精度対応。markerと並ぶオープンソースの有力候補。ただしセットアップが複雑。

**推奨シナリオ:** オープンソースを使いたい、markerよりさらに厳密な構造保持が必要な場合。

### 第5位: **pymupdf4llm**（高速・軽量・ベクターPDF向け）

ベクターPDF（スキャンでない）であれば最速かつ実用十分な品質。表が少ないシンプルなPDFに向く。日本語ベクターPDFであればテキスト抽出精度も高い。

**推奨シナリオ:** 処理速度優先、シンプルなレイアウトのPDF、スキャンでないことが確認できる場合。

### 第6位: **docling**（企業グレード・構造化品質最高）

構造の整然さは最高クラスだが、日本語対応が実験的である点がリスク。将来的な日本語サポートの充実を期待できる。

### 推奨外: pandoc・nougat

- **pandoc**: PDF→Markdown変換がネイティブでなく、精度も低い。本ユースケースには不適。
- **nougat**: 日本語が根本的に非対応。英語論文専用ツールであるため、本ユースケースには使用不可。

---

## 実装提案（ハイブリッドアプローチ）

100ページの日本語PDFを高精度でMarkdown変換する場合、以下のハイブリッドアプローチを推奨する：

**フェーズ1 - 試験変換（無料）:**
まず `pymupdf4llm` でテキスト抽出を行い、結果を確認。ベクターPDFであれば日本語も含めて良好な結果が得られる場合がある。

**フェーズ2 - 高精度変換（GPU環境あり）:**
`marker` を日本語言語指定 (`--langs ja,en`) で実行。必要に応じて`--use_llm`で精度向上。

**フェーズ3 - 最高品質保証（品質最優先）:**
`Claude API` または `Mathpix API` を使用。Claudeは視覚的理解も含めた最高品質の日本語Markdown変換が可能。

---

## 出典

- [marker GitHub (datalab-to)](https://github.com/datalab-to/marker)
- [Surya OCR GitHub](https://github.com/datalab-to/surya)
- [PyMuPDF4LLM 公式ドキュメント](https://pymupdf.readthedocs.io/en/latest/pymupdf4llm/)
- [PyMuPDF4LLM GitHub](https://github.com/pymupdf/pymupdf4llm)
- [docling GitHub](https://github.com/docling-project/docling)
- [IBM Granite-Docling](https://www.ibm.com/granite/docs/models/docling)
- [Mathpix PDF to Markdown](https://mathpix.com/pdf-to-markdown)
- [Mathpix Japanese OCR](https://mathpix.com/blog/japanese-ocr)
- [Mathpix API Pricing](https://mathpix.com/pricing/api)
- [Claude API PDF Support](https://platform.claude.com/docs/en/build-with-claude/pdf-support)
- [nougat GitHub (Meta)](https://github.com/facebookresearch/nougat)
- [MinerU GitHub](https://github.com/opendatalab/MinerU)
- [PDF to Markdown Benchmark (Dr. Leon Eversberg)](https://ai.gopubby.com/benchmarking-pdf-to-markdown-document-converters-fc65a2c73bf2)
- [Best Open Source PDF to Markdown Tools 2026](https://jimmysong.io/blog/pdf-to-markdown-open-source-deep-dive/)
- [PyPI: marker-pdf](https://pypi.org/project/marker-pdf/)
- [PyPI: pymupdf4llm](https://pypi.org/project/pymupdf4llm/)
- [PyPI: mineru](https://pypi.org/project/mineru/)
