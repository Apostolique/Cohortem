\version "2.24.4"
\include "definitions.ly"

title = "{{title}}"
author = "{{author}}"
year = "{{year}}"

% Remove this when you have enough systems.
\paper {
  ragged-last-bottom = ##t
}

% You can pass a list to control how many measures to have per system:
% measures-per-system = #'(3 2 3 2 2 3)
measures-per-system = #'(4)

global = {
  \key c \major
  \time 4/4
  \set Score.currentBarNumber = #1
  \tempo "Allegretto" 4 = 100
}

right = {
  \clef "treble"

  {{right}}
}

left = {
  \clef "bass"

  {{left}}
}

dynamics = {
}

pedal = {
  \set Dynamics.pedalSustainStyle = #'mixed
}

pedal-midi = {
}

piano-music = \create-piano-staff \global \left \right \dynamics \pedal
music-midi = \create-midi \global \left \right \dynamics \pedal-midi

\create-publish \piano-music \music-midi \measures-per-system
\create-debug \piano-music \measures-per-system
