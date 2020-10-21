param(
    [string] $botToken,
    [Int64] $chatID,
    [string] $filePath,
    [string] $caption
)

if ((Get-Command "Send-TelegramLocalDocument" -errorAction SilentlyContinue)) {
  Send-TelegramLocalDocument -BotToken $botToken -ChatID $chatID -File $filePath -Caption $caption
}