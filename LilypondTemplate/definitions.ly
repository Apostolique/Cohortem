\include "articulate.ly"

\header {
  tagline = ##f
}

\paper {
  #(set-paper-size "letter")
  indent = #0
  ragged-last-bottom = ##f
  max-systems-per-page = 6
  top-markup-spacing.padding = 3
  top-system-spacing.padding = 3
  last-bottom-spacing.padding = 3

  oddFooterMarkup = \markup
  \fill-line {
    \center-column {
      \if \on-first-page \fromproperty #'header:copyright
      \unless \on-first-page \fontsize #-4.0 \fromproperty #'header:title
    }
  }
  evenFooterMarkup = \oddFooterMarkup
}


sOn = \sustainOn
sOff = \sustainOff
simile = \markup\fontsize #-2 {simile}
rit = \once {\override TextSpanner.outside-staff-priority = #250 \override TextSpanner.bound-details.left.text = "rit."}
molto-rit = \once {\override TextSpanner.outside-staff-priority = #250 \override TextSpanner.bound-details.left.text = "molto rit."}
ritStart = \startTextSpan
ritEnd = \stopTextSpan
atempo = \tempo \markup \normal-text \italic "a tempo"

% Could be useful if the sheet is meant to be read on a computer screen.
% #(set-global-staff-size 23)

create-piano-staff = #(define-scheme-function (parser location global left right dynamics pedal) (ly:music? ly:music? ly:music? ly:music? ly:music?)
  #{
    \new PianoStaff = "pianostaff" <<
      \new Staff = "RH" << #global #right >>
      \new Dynamics = "dynamics" << #dynamics >>
      \new Staff = "LH" << #global #left >>
      \new Dynamics = "pedal" << #pedal >>
    >>
  #}
)
create-piano-song = #(define-scheme-function (parser location global left right song dynamics pedal) (ly:music? ly:music? ly:music? ly:music? ly:music? ly:music?)
  #{
    \new PianoStaff = "pianostaff" <<
      \new Voice = "RH" << #global #right >>
      << #song >>
      \new Dynamics = "dynamics" << #dynamics >>
      \new Staff = "LH" << #global #left >>
      \new Dynamics = "pedal" << #pedal >>
    >>
  #}
)
create-midi = #(define-scheme-function (parser location global left right dynamics pedal) (ly:music? ly:music? ly:music? ly:music? ly:music?)
  #{
    \unfoldRepeats \articulate
      \new PianoStaff = "pianostaff" <<
        \new Dynamics = "dynamics" << #dynamics >>
        \new Dynamics = "pedal" << #pedal >>
        \new Staff = "lr" << #global #right #left #dynamics #pedal >>
      >>
  #}
)

create-publish = #(define-void-function (parser location piano-music midi-music measures-per-system) (ly:music? ly:music? list?)
  (print-book-with-defaults
    #{
      \book {
        \pointAndClickOff
        \header {
          title = \markup { \title }
          composer = \author
          copyright = \markup \fontsize #-5
          {
            Copyright © \year \author.
          }
        }
        \score {
          #piano-music
          \layout {
            \context {
              \Score
              \consists #(bars-per-line-engraver measures-per-system)
            }
          }
        }

        \score {
          #midi-music
          \midi {
            \context {
              \type "Performer_group"
              \name Dynamics
              \consists "Piano_pedal_performer"
            }
            \context {
              \PianoStaff
              \accepts Dynamics
            }
            \context {
              \Staff
              \consists "Dynamic_performer"
            }
          }
    }
  }
    #}
  ))

create-debug = #(define-void-function (parser location piano-music measures-per-system) (ly:music? list?)
  (print-book-with-defaults
    #{
      \book {
        \pointAndClickOn
        \bookOutputSuffix "debug"
        \header {
          title = \markup { \title " - debug" }
          composer = \author
          copyright = \markup \fontsize #-5
          {
            Copyright © \year \author.
          }
        }
        \score {
          #piano-music
          \layout {
            \context {
              \Score
              \consists #(bars-per-line-engraver measures-per-system)
            }
          }
        }
      }
    #}
  ))

#(define ((bars-per-line-engraver bar-list) context)
  (let* ((working-copy bar-list)
         (total (1+ (car working-copy))))
    `((acknowledgers
       (paper-column-interface
        . ,(lambda (engraver grob source-engraver)
             (let ((internal-bar (ly:context-property context 'internalBarNumber)))
               (if (and (pair? working-copy)
                        (= (remainder internal-bar total) 0)
                        (eq? #t (ly:grob-property grob 'non-musical)))
                   (begin
                     (set! (ly:grob-property grob 'line-break-permission) 'force)
                     (if (null? (cdr working-copy))
                         (set! working-copy bar-list)
                         (begin
                           (set! working-copy (cdr working-copy))))
                           (set! total (+ total (car working-copy))))))))))))
