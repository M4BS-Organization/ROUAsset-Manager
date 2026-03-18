#!/bin/bash
# 等価性検証テスト ローカル実行スクリプト (Issue #29)
#
# 使い方:
#   ./scripts/run-equivalence-tests.sh           # 全テスト実行
#   ./scripts/run-equivalence-tests.sh --build    # ビルドから実行
#   ./scripts/run-equivalence-tests.sh --compile  # コンパイルから実行

set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"
cd "$PROJECT_DIR"

# 色付き出力
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo "=========================================="
echo " 等価性検証テスト (Issue #29)"
echo "=========================================="
echo ""

# --- オプション処理 ---
DO_BUILD=false
DO_COMPILE=false

for arg in "$@"; do
    case $arg in
        --build)   DO_BUILD=true; DO_COMPILE=true ;;
        --compile) DO_COMPILE=true ;;
    esac
done

# --- DataAccess ビルド ---
if [ "$DO_BUILD" = true ]; then
    echo -e "${YELLOW}[1/4] DataAccess ビルド...${NC}"
    msbuild LeaseM4BS/LeaseM4BS.DataAccess/LeaseM4BS.DataAccess.vbproj \
        /p:Configuration=Debug /p:Platform="AnyCPU" /m /verbosity:quiet
    echo -e "${GREEN}  ビルド完了${NC}"
    echo ""
fi

# --- DLL コピー ---
DLL_PATH="LeaseM4BS/LeaseM4BS.DataAccess/bin/Debug/LeaseM4BS.DataAccess.dll"
if [ ! -f "$DLL_PATH" ]; then
    echo -e "${RED}ERROR: $DLL_PATH が見つかりません。--build オプションで実行してください。${NC}"
    exit 1
fi
cp "$DLL_PATH" .

# --- VBCコンパイラ検出 ---
VBC_PATH=""
for p in "/c/Program Files/Microsoft Visual Studio/"*/*/MSBuild/Current/Bin/Roslyn/vbc.exe; do
    if [ -f "$p" ]; then
        VBC_PATH="$p"
        break
    fi
done

if [ -z "$VBC_PATH" ]; then
    echo -e "${RED}ERROR: Roslyn VBC コンパイラが見つかりません${NC}"
    exit 1
fi

# --- コンパイル ---
if [ "$DO_COMPILE" = true ] || [ ! -f "test_equivalence.exe" ] || [ ! -f "test_output_diff.exe" ]; then
    echo -e "${YELLOW}[2/4] テストスクリプト コンパイル...${NC}"

    "$VBC_PATH" \
        -reference:LeaseM4BS.DataAccess.dll \
        -reference:System.Data.dll \
        -out:test_equivalence.exe \
        test_equivalence.vb 2>&1 | tail -1

    "$VBC_PATH" \
        -reference:LeaseM4BS.DataAccess.dll \
        -reference:System.Data.dll \
        -out:test_output_diff.exe \
        test_output_diff.vb 2>&1 | tail -1

    echo -e "${GREEN}  コンパイル完了${NC}"
    echo ""
fi

# --- 等価性テスト実行 ---
echo -e "${YELLOW}[3/4] 等価性テスト実行...${NC}"
EQUIV_EXIT=0
./test_equivalence.exe 2>&1 | tee test_equivalence_result.txt || EQUIV_EXIT=$?
echo ""

# --- 出力差分テスト実行 ---
echo -e "${YELLOW}[4/4] 出力差分テスト実行...${NC}"
DIFF_EXIT=0
./test_output_diff.exe 2>&1 | tee test_output_diff_result.txt || DIFF_EXIT=$?
echo ""

# --- 結果サマリ ---
echo "=========================================="
echo " テスト結果サマリ"
echo "=========================================="

EQUIV_SUMMARY=$(tail -3 test_equivalence_result.txt | head -1)
DIFF_SUMMARY=$(tail -3 test_output_diff_result.txt | head -1)

echo "  計算等価性: $EQUIV_SUMMARY"
echo "  出力差分:   $DIFF_SUMMARY"
echo ""

if [ $EQUIV_EXIT -ne 0 ] || [ $DIFF_EXIT -ne 0 ]; then
    echo -e "${RED}テスト失敗あり (exit code: equiv=$EQUIV_EXIT, diff=$DIFF_EXIT)${NC}"
    exit 1
else
    echo -e "${GREEN}全テスト PASS${NC}"
    exit 0
fi
