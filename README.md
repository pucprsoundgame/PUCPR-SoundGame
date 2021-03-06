<a href="https://twitter.com/chrisdbhr"><img src="https://img.shields.io/twitter/follow/chrisdbhr.svg?style=social&amp;label=Follow&amp;maxAge=2592000" alt="Follow @chrisdbhr" data-pin-nopin="true"></a>

# Jogo Modular
Alguns arquivos do jogo ficam expostos em uma pasta para modificação fácil e rápida pelos alunos.

[![alt text](https://github.com/pucprsoundgame/PUCPR-SoundGame/raw/master/botao_window.png "Baixar para Windows")](https://github.com/pucprsoundgame/PUCPR-SoundGame/raw/master/Builds/Windows/PSG.zip)
[![alt text](https://github.com/pucprsoundgame/PUCPR-SoundGame/raw/master/botao_jogaronline.png "Jogar online")](https://pucprsoundgame.netlify.com)

## Instruções (Windows)
- Baixe a [versão para Windows](https://github.com/pucprsoundgame/PUCPR-SoundGame/raw/master/Builds/Windows/Windows.zip) e descompacte em uma pasta.
- Substitua os arquivos de som da pasta **StreamingAssets** com os que você fizer.
- Os arquivos que tem um número no final são os que podem ter variações. Adicione quantos quiser, seguindo o padrão dos nomes.
- A pasta **Jukebox** aceita vários arquivos de varios nomes.
- **Todos** os sons **devem** ser no formato **OGG**.

## Observações
- Se você quer usar o código fonte do jogo, saiba que ele não tem foco em performance. A maneira que ele carrega o som é lenta pois os sons são carregados direto do HD, e não compilados em bundles como a Unity faz com todos os arquivos. Use apenas para estudo.

## Notas da versão

**1.71 - 26 de maio de 2020**
- Build na Unity 2019.4.15
- Nova hierarquia de pastas
- Versão MAC removida, não é mais suportada.
- Corrigido problema em versao web sem som.

**0.51 - 27 de fevereiro de 2019**
- Build na Unity 2018.3.6
- Build MAC.

**0.50 - 07 de agosto de 2018**
- Incluido novas imagens no jogo.
- Incluido código de câmera para possibilitar movimento horizontal no mapa e mapas maiores.

**0.40 - 14 de maio de 2018**
- Adicionado telas de **Menu** e **GameOver*.
- Adicionado sons para serem customizados:
	- Musica da tela de Menu.
	- Musica da tela de GameOver.
	- Som (SFX) ao dar GameOver.
	- Som (SFX) do fogo no buraco.
- Adicionado buraco no jogo para que o jogador possa cair de "dar GameOver".
