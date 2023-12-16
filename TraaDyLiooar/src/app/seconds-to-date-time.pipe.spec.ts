import { SecondsToDateTimePipe } from './seconds-to-date-time.pipe';

describe('SecondsToDateTimePipe', () => {
  it('create an instance', () => {
    const pipe = new SecondsToDateTimePipe();
    expect(pipe).toBeTruthy();
  });
});
