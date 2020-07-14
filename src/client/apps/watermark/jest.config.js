module.exports = {
  name: 'watermark',
  preset: '../../jest.config.js',
  coverageDirectory: '../../coverage/apps/watermark',
  snapshotSerializers: [
    'jest-preset-angular/AngularSnapshotSerializer.js',
    'jest-preset-angular/HTMLCommentSerializer.js'
  ]
};
