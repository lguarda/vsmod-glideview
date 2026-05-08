# Changelog

## [1.1.0] - 2026-05-08
### Changed
- default mod is actually not third person but overhead, which is the same expect you can rotate camera freely
### Added
- the mod now also trigger when sitting, but only if you are not looking at your feet (so you can sit to do clayforming)
- now the mod behavior can be configured via glideview.json, it's pretty self explanatory for now and it looks like this
```
{
  "default_view": "first", # "first is for first person"
  "elk_view": "overhead", # "first is the thrid person with free cam"
  "boat_view": "first",
  "glide_view": "overhead",
  "other_mount_view": "third",
  "sit_view": "overhead",
  "sit_pitch": -20 # here this is in degree where -90 is looking down 0 is the horizon and 90 looking straight up
}
```

## [1.0.0] - 2026-05-06
### Added
- initial release with base working principle
